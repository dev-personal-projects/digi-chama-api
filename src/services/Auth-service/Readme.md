# Authentication Microservice for Digichame

## Overview
This microservice handles user authentication for the Digichame application, providing secure registration, login, and verification processes for users. It's built using a clean architecture approach with .NET and Azure Cosmos DB, integrating with external notification services for email and SMS verification.

## Architecture

The microservice follows a layered architecture pattern:

```
AuthMicroservice/
├── src/
│   ├── Auth.API                      # API endpoints and configuration
│   ├── Auth.Core                     # Business logic and domain models
│   ├── Auth.Infrastructure           # External dependencies and implementations
│   └── Auth.Common                   # Shared utilities
└── tests/                            # Test projects
```

### Key Components

#### Auth.API
Contains the API controllers, middleware, and application configuration:
- `AuthController`: Handles registration, login, verification, and other authentication endpoints
- `Startup.cs`: Configures service dependencies and middleware pipeline

#### Auth.Core
Contains the business logic and domain models:
- `User.cs`: Primary entity model containing authentication-related properties
- `AuthService.cs`: Core authentication business logic
- `TokenService.cs`: JWT token generation and validation
- `OtpService.cs`: OTP generation and validation logic

#### Auth.Infrastructure
Implements interfaces defined in the core layer:
- `UserRepository.cs`: Cosmos DB data operations for user management
- `EventPublisher.cs`: Azure Service Bus integration for publishing auth events
- `NotificationService.cs`: Handles sending notifications via email or SMS

## Data Model

### User Entity
The central model for authentication with the following key fields:
- `Id`: Unique identifier
- `FullName`: User's full name
- `Email/Phone`: Contact method based on user preference
- `AuthProvider`: Email, Phone, or OAuth provider
- `PasswordHash`: Securely stored password hash
- `IsVerified`: Verification status
- `VerificationCode`: OTP for account verification
- `InitialRole`: Default "not allocated" role
- `Location`: User's location information
- `ProfilePictureUrl`: Optional profile image

## Authentication Flow

1. **Registration**:
   - User provides full name, email/phone, and password
   - System creates unverified user account
   - OTP sent to email or phone
   - Event published: `UserCreatedEvent`

2. **Verification**:
   - User submits OTP
   - System validates OTP and marks account as verified
   - JWT token issued for authenticated session
   - Event published: `UserVerifiedEvent`

3. **Login**:
   - User provides email/phone and password
   - System validates credentials
   - JWT token issued for authenticated session

4. **OAuth Flow**:
   - User authorizes via Google OAuth
   - System retrieves user information from Google
   - Account created or existing account retrieved
   - JWT token issued for authenticated session

5. **Role Management**:
   - When user creates a chama group: role changes to "admin"
   - When user joins a group: role changes to "member"
   - Event published: `UserRoleChangedEvent`

## Messaging System

The microservice uses Azure Service Bus for event-driven communication with other microservices. This ensures loose coupling and reliable message delivery.

### Event Publishing

When significant authentication events occur, the service publishes events to Azure Service Bus topics:

1. `EventPublisher.cs` serializes event data to messages
2. Messages are sent to predefined topics:
   - `user-created`: When a new user registers
   - `user-verified`: When a user completes verification
   - `user-role-changed`: When a user's role changes

### Subscription Pattern

Other microservices (e.g., Group Management) subscribe to these topics to react to authentication events:

1. Group Management service subscribes to `user-role-changed`
2. When a user creates a group and becomes an admin, the Group Management service receives the event
3. Group Management updates its database accordingly

This pattern maintains separation of concerns while ensuring data consistency across microservices.

## External Services Integration

### Email Service
- Uses the selected .NET email library to send verification emails and notifications
- Configurable templates for different notification types

### SMS Service
- Integrates with Africa's Talking API for SMS delivery
- Sends verification codes and notifications to users who register with phone numbers

## Configuration

Key configuration settings required for deployment:
- Cosmos DB connection string
- JWT secret and token lifetime
- Azure Service Bus connection string
- Email service credentials
- Africa's Talking API credentials

## Development Setup

1. Install .NET 6.0+ SDK
2. Configure user secrets or local environment variables
3. Run database migrations
4. Start the service: `dotnet run --project src/Auth.API`

## Testing

The solution includes:
- Unit tests for core business logic
- Integration tests for API endpoints
- Mock services for external dependencies

Run tests with: `dotnet test`





##Alogrithms and best practices
Algorithms & Best Practices

Password Security

Use BCrypt or Argon2id for password hashing
Implement rate limiting for login attempts
Consider multi-factor authentication for high-value operations


OTP Generation & Validation

Use secure random number generation
Short expiration time (5-10 minutes)
Rate limit OTP requests
Consider TOTP (Time-based OTP) standards


Token Management

Short-lived access tokens (15-60 minutes)
Refresh token rotation
Token blacklisting for revoked sessions


Performance Optimization

Cache frequently accessed user data
Use Azure Front Door or similar for edge caching of static resources
Implement database connection pooling


Security Best Practices

Implement OWASP security recommendations
Use HTTPS for all communications
Store secrets in Azure Key Vault
Regular security audits and penetration testing


Microservice Communication

Use event-driven architecture for cross-service notifications
Implement circuit breakers for service-to-service communication
Consider Azure Service Bus for reliable message delivery


DRY (Don't Repeat Yourself)

Create abstraction layers for notification delivery (email/SMS)
Implement generic repository pattern for data access
Use shared DTOs for common data structures


Observability

Implement structured logging (Serilog or NLog)
Use Application Insights for telemetry
Implement health checks for service status monitoring
Create dashboards for key metrics (login success rate, OTP delivery success)


## Authentication Microservice Design for Digichame
Let's design a comprehensive authentication flow for your microservice architecture, focusing on the Cosmos DB implementation and best practices.
Authentication Flow

User Registration

User submits registration form with full name, email/phone, password
System validates input
System generates OTP (One-Time Password)
OTP sent to email or phone based on user's chosen contact method
User account created in database with status "unverified"


OTP Verification

User enters OTP received
System validates OTP against stored value
If valid, user status updated to "verified" with role "not allocated"
JWT token issued for authenticated session


Social OAuth Flow (Google)

User selects "Continue with Google"
System redirects to Google OAuth
Upon successful authentication, retrieve user info from Google
Check if user exists in database

If exists: login
If new: create account with status "verified" (role "not allocated")




Role Assignment

When user creates a chama group:

Create group in groups database
Create membership record with userId, groupId, role="admin"


When user joins a group via invitation:

Create membership record with userId, groupId, role="member"




Authentication for API Access

JWT token validation for protected endpoints
Token contains basic user info and timestamp
Role-based authorization checked against membership database



Email Integration for .NET
For sending emails in a .NET environment, consider:

MailKit - Modern, well-maintained library with MIME support
SendGrid Client Library - If you prefer a third-party service
Microsoft.Extensions.Email - Simple, integrated with .NET ecosystem

SMS Integration

Africa's Talking API is an excellent choice for the region
Create an abstraction layer to handle both email and SMS messages uniformly

Database Considerations
Cosmos DB is suitable for your authentication microservice because:

Scalability - Handles high-volume authentication requests
Global distribution - Low-latency access across regions
Schema flexibility - Accommodates varied user attributes
Security features - Data encryption at rest and in transit

Alternative databases to consider:
authentication to use cosmos