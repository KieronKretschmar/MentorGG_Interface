# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).


## [0.2.0] - 2020-03-11
### Added
- Console Timestamps.
- Paddle Webhook Intergration.
- Routes for Match Retreiver and Demo Central.
- Development CORS.
### Changed
- Rename env var MYSQL_CONNECTIONSTRING to MYSQL_CONNECTION_STRING.
- Fix Signout endpoint.

## [0.1.1] - 2020-01-24
### Added
- Routes for FaceItMatchGatherer and SharingCodeGatherer.
- MySQL Connections retries.
### Changed
- Net Core Framework to 3.1.
### Removed
- Let's Encrpyt SSL (SSL is now configured with the Ingress on K8s).


## [0.1.0] - 2020-01-02 
### Added 
- Swagger documentation.
- Identity authentication (Steam).
- User database.
- Let's Encrypt SSL.
