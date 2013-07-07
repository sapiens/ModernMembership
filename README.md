ModernMembership
================

A modern asp.net membership, totally unrelated to the MS one. Work in progress.


## License

Apache 2.0


##Why Modern

- Developed using TDD
- DDD support via Domain Events i.e every model change will generate (but not send) a domain event
- Designed with CQRS in mind. For easy querying use events to generate the read model specific to your app needs.
- Support for integrating with third party sites 
- Storage agnostic. Persistence is abstracted via Repository or Services
- Will support rights based authorization (besides role based, here called groups), including support for multi tenancy


That's it for now :)
