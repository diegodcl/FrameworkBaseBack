---
name: "FrameworkBase"
applyTo: '**'
description: "Base framework for building C# modular architected applications using .NET 9, DDD, and other main design principles."
---

# Copilot Project Instructions

This project is called "FrameworkBase". Use this name when referring to the project in documentation, code comments, and generated content.

## Project Overview

This project is intended to be a base framework for building C# modular architected applications using .net 9, DDD, and other main design principles. The project is structured to support modular development, allowing for easy addition and management of features and services.

## Reference terms

[const moduleName] reference to the name of a module in the project, such as "Finance", "Events", "Maintenance", etc.
[const entityName,modelName] reference to the name of an entity in the project, such as "User", "Event", "MaintenanceRequest", etc.
[const serviceName] reference to the name of a service in the project, such as "AccountService", "EventService", "MaintenanceService", etc.
[const featureName] reference to the name of a feature in the project, such as "CreateAccount", "GetEvents", "UpdateMaintenanceRequest", etc.
[const endpointName] reference to the name of an application endpoint in the project, such as "Account", "Events", "Organization", etc.
[const Feature] reference to the name of a feature in the project, such as "Create", "Read", "Update", "Delete", etc.
[const Entity] reference to the name of an entity in the project, such as "User". Entities are implemented as classes in the Domain layer and have corresponding DTOs in the Application layer.


- [backend module implementation instructions](./instructions/module-authoring.definitive.instructions.md)
- [frontend CRUD UI implementation instructions](./instructions/frontend-crud.definitive.instructions.md)
