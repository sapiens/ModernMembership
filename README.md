ModernMembership
================

A modern asp.net membership, totally unrelated to the MS one. Work in progress.


## License

Apache 2.0


##Why Modern

- Developed using TDD
- DDD support via Domain Events i.e every model change will generate (but not send) a domain event
- Designed with CQRS in mind. For easy querying use events to generate the read model specific to your app needs.
- Backend support for integrating with third party sites (but not actual integration)
- Storage agnostic. Persistence is abstracted via Repository or Services
- Supports rights based authorization, including support for multi tenancy (rights are global or tenant scoped)
- Members can be global or locally scoped (multi tenant scenario)


## Is It For You?

Only if you are comfortable with
-  Message driven architecture (including using a service bus)
-  Eventual consistency
-  Autonomus components architecture (based on what Udi Dahan calls  ABC - Autonomous Business Components)
-  CQRS

That's it for now :)
