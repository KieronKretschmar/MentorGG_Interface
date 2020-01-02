#!/bin/bash

# Assembly location from the current directory
RELEASE_ASSEMBLY="MentorInterface/bin/Release/MentorInterface.dll"

# Version of the docs to publish
RELEASE_DOC_VER="v1"

# Location of the Docs directory
DOCS_DIR="Docs"

# File name of the interface json
INTERFACE_FNAME="interface.json"

# Make the Documentation Directory
mkdir -p $PWD/$DOCS_DIR

# Render the INTERFACE_FNAME file containing the OPEN_API Spec
dotnet swagger tofile --output $PWD/$DOCS_DIR/$INTERFACE_FNAME $PWD/$RELEASE_ASSEMBLY $RELEASE_DOC_VER


# REQUIRED: https://github.com/bootprint/bootprint-openapi#overview
# Render the OPEN_API Spec to HTML Docs
bootprint openapi $PWD/$DOCS_DIR/$INTERFACE_FNAME $PWD/$DOCS_DIR/Build/
