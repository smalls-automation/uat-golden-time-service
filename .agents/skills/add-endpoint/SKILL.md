---
name: add-endpoint
description: Add a new REST endpoint to an existing router and wire it through the service layer
---

# Skill: Add Endpoint

## Steps

1. **Define request/response schemas** in `schemas/{resource}.py`
   - Use Pydantic `BaseModel` with typed fields
   - Response model gets `model_config = ConfigDict(from_attributes=True)`

2. **Add the service method** to `I{Resource}Service` and `{Resource}Service`
   - Return `Result[{Resource}Response]`
   - Keep under 15 lines

3. **Add the router endpoint** in `routers/{resource}.py`
   - Correct HTTP decorator (`@router.post`, `@router.put("/{id}")`, etc.)
   - `Annotated[..., Depends(...)]` for the service
   - Pattern match `Result` → `HTTPException` on `Err`

4. **Verify**: `pytest` exits 0 (or `pytest -x` if tests exist for this router)

## Do not
- Create a new router file for a single endpoint on an existing resource
- Put logic in the router beyond service delegation and error mapping
- Modify files outside `routers/`, `services/`, and `schemas/`
