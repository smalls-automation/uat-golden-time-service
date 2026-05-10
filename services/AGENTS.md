# services/AGENTS.md

Services own all business logic. Each service is a class with an abstract base
(interface). Routers and other services depend on the abstract base only.

## Naming
- Abstract base: `I{Resource}Service` in `services/base.py` or `services/i_{resource}_service.py`
- Implementation: `{Resource}Service` in `services/{resource}_service.py`

## Structure
```python
from abc import ABC, abstractmethod

class I{Resource}Service(ABC):
    @abstractmethod
    async def get_async(self, id: UUID) -> Result[{Resource}]:
        ...

class {Resource}Service(I{Resource}Service):
    def __init__(self, repository: I{Resource}Repository) -> None:
        self._repository = repository

    async def get_async(self, id: UUID) -> Result[{Resource}]:
        entity = await self._repository.get_by_id_async(id)
        if entity is None:
            return Err(f"{id} not found")
        return Ok(entity)
```

## Rules
- Return `Result[T]` (`Ok` / `Err`) for operations with meaningful failure modes
- Inject repository abstractions only — never the ORM session directly
- Keep methods under 15 lines; extract named helpers for longer logic
- No FastAPI types (Request, Response, HTTPException) inside services

## Adding a new service
1. Create the abstract base and implementation in `services/`
2. Register the dependency provider in `dependencies.py`:
   ```python
   def get_{resource}_service(repo=Depends(get_{resource}_repo)) -> I{Resource}Service:
       return {Resource}Service(repo)
   ```
