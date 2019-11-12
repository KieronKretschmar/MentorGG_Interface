# Mentor Interface

A REST API providing authentication and access to the Mentor Engine

# Building

`docker build -t mentorinterface:<version> .`

# Run the docker image

`docker run --name mi_container --rm -d -p 80:80 mentorinterface:<version>`
