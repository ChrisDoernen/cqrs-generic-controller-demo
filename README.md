# Maybe you can delete all controllers in your API.

## Why?

If you are using [MediatR](https://github.com/jbogard/MediatR) (or a similar package), you are probably doing CQS/CQRS.
The idea is: Every request (read command or query) is going to one single class, the mediator.
Then why do we need so many controllers, if they are just calling the mediator anyway?

"Routing, serialization, authorization!" you say - and you are right: These are responsibilities of controllers, but let's take a look.

**Routing**  
In many scenarios, DDD is more appropriate than thinking in resources like in classical REST Api's.
It just feels natural to have the requests named after the business domain like "GetProducts", "StartCharging", "CancelMeeting".
So why not use them as routes?
The result is an RPC-ish Api, with a single endpoint taking all the requests. 
I always admired the simplicity of GraphQL with just one endpoint.

**Serialization**  
Yes, we do have to implement this ourselves instead of letting the framework do it's thing, but we only have to do this in one place.

**Authorization**  
In Clean Architecture (CA), we want to separate business (domain) logic from infrastructure code.
Is authorization business or infrastructure?
Well, there are two aspects: The mechanism and the roles, permissions, policies itself.
The latter are clearly defined by the business requirements, so it is reasonable to move this into the business layer.
With MediatR we can implement those cross-cutting concerns very elegantly with so called "Behaviors".
The authorization policies are then placed next to the request handlers, resulting in nice coherence of business things that belong together.
The mechanism is of course depending on the environment but in the end, there will be some authorized user object with a set of roles, to be evaluated by the authorization policy for a given request.


## About this repo

This .NET Demo implementation is just a proof-of-concept. 
It leaves out many topics (e.g. authorization, versioning, documentation, ...) for brevity.
Even handling file up- and download (multipart forms) is possible within one controller.
I chose the minimal api style for simplicity but of course, a classical controller does the job as well.

After cloning, you can run `dotnet test` in the test project or debug to see how things work.


