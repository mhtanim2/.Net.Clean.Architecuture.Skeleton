# Claude Code Development Workflow

## 🔄 Step-by-Step Development Process

### Phase 1: Analysis (Read First!)

#### 1.1 Understand the Request
- **ALWAYS** re-read the user's request multiple times
- Identify all requirements
- Ask clarifying questions if anything is unclear
- Confirm understanding before proceeding

#### 1.2 Analyze Existing Codebase
- **NEVER** start coding without analysis
- Use the Task tool to explore the project structure:
  ```
  Task: Analyze the project structure and existing patterns
  ```
- Look for:
  - Similar existing features
  - Established patterns
  - Naming conventions
  - File organization
  - Used libraries

#### 1.3 Check Architecture Rules
- Verify layer dependencies
- Identify where new code should go
- Check for existing interfaces or base classes

### Phase 2: Planning

#### 2.1 Create Todo List
- **ALWAYS** create a TodoWrite list for complex tasks
- Break down into small, specific steps
- Update status as you progress

#### 2.2 Plan the Solution
- Identify all files that need to be created/modified
- Plan the order of operations
- Consider dependencies between files

#### 2.3 Verify Plan
- Ensure plan follows Clean Architecture rules
- Check for consistency with existing code
- Consider testability

### Phase 3: Implementation

#### 3.1 Domain Layer First (if new entity)
1. Create/update domain entity
2. Ensure it inherits from BaseEntity
3. Add domain logic within the entity
4. NO external dependencies

#### 3.2 Application Layer
1. **DTOs** - Create data transfer objects
2. **Interfaces** - Create repository/service interfaces
3. **Commands/Queries** - Create CQRS objects
4. **Validators** - Create FluentValidation validators
5. **Handlers** - Create MediatR handlers
6. **Mappings** - Update/create AutoMapper profiles

#### 3.3 Persistence Layer
1. **Repository Implementation** - Implement repository interfaces
2. **Configuration** - Create EF Core entity configuration
3. **DbContext** - Update with new DbSets if needed

#### 3.4 Infrastructure Layer (if needed)
1. **Service Implementation** - Implement external service interfaces
2. **Configuration** - Add service registration

#### 3.5 API Layer
1. **Controller** - Create/update controller
2. **DTOs** - Create request/response DTOs
3. **Endpoints** - Implement HTTP endpoints

### Phase 4: Testing and Validation

#### 4.1 Code Review Checklist
- [ ] Follows Clean Architecture rules
- [ ] No forbidden patterns used
- [ ] All dependencies correct
- [ ] Naming conventions followed
- [ ] Validation implemented
- [ ] Error handling implemented
- [ ] Documentation added

#### 4.2 Build Verification
- Check for compilation errors
- Verify all references correct
- Ensure no circular dependencies

## 📋 Mandatory Workflow Steps for ANY Task

### 1. Initial Setup
```bash
# Always check current directory
pwd

# Verify project structure
ls -la

# If working on specific project, navigate there
cd path/to/project
```

### 2. Analysis Commands
```bash
# For exploring project structure
find . -name "*.cs" | grep -E "(Controller|Service|Repository)" | head -20

# For finding similar features
grep -r "class.*Controller" --include="*.cs" .

# For checking dependencies
grep -r "using.*Application" --include="*.cs" .
```

### 3. Implementation Order
1. **Domain Entity** (if new)
2. **Application Layer** (DTOs, Commands, Queries, Handlers)
3. **Persistence Layer** (Repository implementation)
4. **API Layer** (Controller)
5. **Tests** (Unit tests first, then integration)

## 🎯 Specific Task Templates

### Template: Adding New Entity

```csharp
// 1. Domain Layer
public class YourEntity : BaseEntity
{
    // Properties
    // Domain methods
}

// 2. Application Layer
// - DTOs
// - Repository interface
// - Command/Query classes
// - Handlers
// - Validators
// - Mapping profile

// 3. Persistence Layer
// - Repository implementation
// - Entity configuration

// 4. API Layer
// - Controller with CRUD endpoints
```

### Template: Adding New Endpoint

```csharp
// 1. Check if existing controller exists
// 2. Create command/query if needed
// 3. Create validator
// 4. Create handler
// 5. Update controller
// 6. Test endpoint
```

### Template: Fixing Bug

1. **Locate the bug**:
   - Search for related code
   - Understand the flow
   - Identify root cause

2. **Plan the fix**:
   - Minimal changes
   - Don't break existing functionality
   - Consider edge cases

3. **Implement**:
   - Fix the issue
   - Add tests if needed
   - Verify fix works

4. **Verify**:
   - Run existing tests
   - Test the fix
   - Check for regressions

## 🚨 Critical Rules

### ALWAYS Do:
1. **Read the entire request** before starting
2. **Explore the codebase** to understand patterns
3. **Create a plan** before coding
4. **Use TodoWrite** for tracking
5. **Follow layer rules** strictly
6. **Validate all input** with FluentValidation
7. **Use repositories** for data access
8. **Handle exceptions** appropriately

### NEVER Do:
1. **Skip analysis** and start coding immediately
2. **Break layer dependencies**
3. **Add business logic to controllers**
4. **Skip validation**
5. **Use magic numbers or strings**
6. **Create circular dependencies**
7. **Commit without testing**
8. **Hard-code configuration values**

## 🔄 Review Process

### Self-Review Checklist
- [ ] Code follows all patterns
- [ ] No compilation errors
- [ ] All layers correctly separated
- [ ] Tests pass
- [ ] Documentation updated
- [ ] No TODOs left (unless intentional)

### Final Verification
1. Build the solution
2. Run all tests
3. Check API endpoints with Swagger
4. Verify error scenarios
5. Confirm requirements met

## 📝 Common Pitfalls to Avoid

1. **Rushing to code** without understanding
2. **Not checking existing patterns**
3. **Breaking Clean Architecture rules**
4. **Forgetting validation**
5. **Not handling errors**
6. **Creating tight coupling**
7. **Missing tests**
8. **Poor naming conventions**

Remember: **Slow is smooth, smooth is fast**. Take time to understand and plan before coding!