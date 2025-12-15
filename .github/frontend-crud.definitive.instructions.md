# Frontend CRUD Authoring Guide (Next.js, Condominio)

This guide standardizes how to build CRUD UIs (lists, forms, actions) for Next.js in the Condominio project, based on existing patterns (Organization/Customer, Property, Documents) and the general CRUD instructions. It is step-by-step and preserves folder structure and conventions. Use Zod for validation.

## Folder & Naming Conventions
- Domain types: `frontend/src/domain/[entity]/[Entity].ts`
- Domain actions (server actions): `frontend/src/domain/[entity]/actions/*.ts`
- Forms (client components): `frontend/src/domain/[entity]/forms/Form[Entity].tsx`
- DAL client: `frontend/src/dal/[entity]/[entity]Api.ts`
- Pages/CRUD container: `frontend/src/app/(admin)/(others-pages)/(cadastros)/[entity]/page.tsx` and `[Entity]Crud.tsx`
- Registration: add your form component to `frontend/src/components/crud/FormComponents.tsx`
- Shared components: reuse `InputField`, `Label`, `Button`, `DataTable`, etc.

## Step 1: Define Domain Types
Create `src/domain/[entity]/[Entity].ts`:
```ts
export interface [Entity] {
  id: string;
  // fields...
  createdAt?: string;
  updatedAt?: string;
}

export interface Create[Entity]Payload {
  // input fields; no id
}

export interface Update[Entity]Payload extends Create[Entity]Payload {}
```
Reference examples: `src/domain/property/Property.ts`, `src/domain/organization/Customer.ts`.

## Step 2: DAL API Client
Create `src/dal/[entity]/[entity]Api.ts` using `BaseApiClient`:
```ts
import BaseApiClient from "../BaseApiClient";
import { [Entity], Create[Entity]Payload, Update[Entity]Payload } from "@/domain/[entity]/[Entity]";

export class [Entity]Api extends BaseApiClient {
  async getAll(): Promise<[Entity][]> { return this.request<[Entity][]>(`/[entity]`); }
  async getById(id: string): Promise<[Entity] | null> {
    try { return await this.request<[Entity]>(`/[entity]/${id}`); } catch { return null; }
  }
  async create(payload: Create[Entity]Payload): Promise<[Entity]> {
    return this.request<[Entity]>(`/[entity]`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload),
    });
  }
  async update(id: string, payload: Update[Entity]Payload): Promise<[Entity]> {
    return this.request<[Entity]>(`/[entity]/${id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload),
    });
  }
  async delete(id: string): Promise<void> {
    await this.request<void>(`/[entity]/${id}`, { method: "DELETE" });
  }
}
```
Adjust paths to match backend endpoints.

## Step 3: Server Actions
Create server actions in `src/domain/[entity]/actions/` using `buildApiUrl` and the DAL client, following Organization/Property patterns:
- `get[Entities].ts`: fetch list
- `get[Entity].ts`: fetch single
- `create[Entity]Action.ts`: call API create, return `{ success, message }`
- `update[Entity]Action.ts`: call API update
- `delete[Entity]Action.ts`: call API delete

Pattern (create):
```ts
"use server";
import { buildApiUrl } from "@/lib/subdomainResolver";
import { [Entity]Api } from "@/dal/[entity]/[entity]Api";
import { Create[Entity]Payload } from "@/domain/[entity]/[Entity]";

export async function create[Entity]Action(_: any, formData: FormData) {
  const apiUrl = await buildApiUrl();
  const api = new [Entity]Api(apiUrl);
  // map formData -> payload
  const payload: Create[Entity]Payload = { /* ... */ };
  try { await api.create(payload); return { success: true, message: "[Entity] criado com sucesso" }; }
  catch (error: any) { return { success: false, message: error?.message ?? "Erro" }; }
}
```
Use similar shape for update/delete. For file uploads, send `FormData` and avoid `Content-Type` header (let the browser set boundary).

## Step 4: Form Component with Zod Validation
Create `src/domain/[entity]/forms/Form[Entity].tsx` as a client component:
- Import `useActionState`, server actions, `CrudStep`, `toast`, `zod`.
- Define `Zod` schema for fields (strings, emails, uuid, etc.). Example from `FormPolicy` and `FormDocument`.
- On submit, parse `Object.fromEntries(formData)` (or custom shaping) with the schema; if invalid, toast error and return.
- Call create/update server actions in transitions; pass `id` when updating.
- Render inputs with `Label` and `Input`; for selects, reuse existing select components.
- Preserve required fields with `required` and placeholders; use `defaultValue` when editing.

Example skeleton:
```tsx
const Schema = z.object({
  id: z.string().optional(),
  name: z.string().min(2).max(100),
  description: z.string().max(500).optional().nullable(),
  email: z.string().email().optional(),
});

export default function Form[Entity](props: FormProps) {
  const { model, setStep, selectedItem } = props;
  const [createState, createAction, isCreatePending] = useActionState(create[Entity]Action, { success: false, message: "" });
  const [updateState, updateAction, isUpdatePending] = useActionState(update[Entity]Action, { success: false, message: "" });

  const handleSubmit = async (formData: FormData) => {
    const parsed = Schema.safeParse(Object.fromEntries(formData));
    if (!parsed.success) { toast.error("Falha na validação"); return; }
    if (selectedItem?.id) { formData.append("id", selectedItem.id); React.startTransition(() => updateAction(formData)); }
    else { React.startTransition(() => createAction(formData)); }
  };

  useEffect(() => { /* toast success/error like FormPolicy */ }, [createState, updateState, setStep]);

  return (
    <form action={handleSubmit} className="space-y-4">
      {/* inputs */}
      <Button type="submit" disabled={isCreatePending || isUpdatePending}>
        {selectedItem ? "Atualizar" : "Criar"}
      </Button>
    </form>
  );
}
```

## Step 5: CRUD Table/Page
- Create `[Entity]Crud.tsx` under the page folder; use `react-data-table-component` like `DocumentsCrud`/`CondominiosCrud`.
- Columns include actions (Edit/Delete) invoking `setSelectedItem` and server delete action.
- For links (e.g., document URL), render anchors with `target="_blank"` and download when applicable.
- Page `page.tsx` loads data (via server action) and renders the CRUD component with props.

## Step 6: Register Form
Add to `src/components/crud/FormComponents.tsx` map:
```ts
import Form[Entity] from "@/domain/[entity]/forms/Form[Entity]";
export const formComponents: Record<string, any> = {
  ...,
  "[entity]": Form[Entity],
};
```
Ensure the model key matches usage in CRUD pages.

## Step 7: Validation Patterns
- Use Zod schemas in forms; validate before invoking server actions.
- For nested objects (e.g., Address), flatten keys (`"address[line1]"`) and map back in server actions.
- On server actions, you can re-validate payload shape if needed.

## Step 8: Error/Toast Handling
- On validation failure: toast error and do not submit.
- On server error response: catch and toast `error?.message ?? "Erro"`.
- On success: toast success and set `CrudStep.Idle/Read` as in existing forms.

## Step 9: Revalidation / Refresh
- After create/update/delete, optionally call `revalidatePath` or rely on client state refresh (pattern varies by existing actions). Follow existing implementations in `organization` and `property` actions.

## Step 10: Misc Patterns
- Inputs: use shared `InputField` (supports `accept` for files), `Label`, `Button`.
- File uploads: use `FormData` and upload endpoints (see Documents module); do not set `Content-Type` manually.
- Tables: use `customStyles` from `components/tables/DataTables/customStyle`.
- Keep text in Portuguese for UX consistency (e.g., toast messages in existing forms).

By following these steps, new CRUD UIs will align with existing Organization and Property implementations and the backend contract.

- Frontend crud implemented to use as reference:
  - Organization/Customer: `frontend/src/domain/organization/forms/FormCustomer.tsx`, `frontend/src/app/(admin)/(others-pages)/(cadastros)/customer/CustomerCrud.tsx`, `frontend/src/dal/customer/customerApi.ts`, `frontend/src/domain/organization/actions/`, 
  - Property: `frontend/src/domain/property/forms/FormProperty.tsx`, `frontend/src/app/(admin)/(others-pages)/(cadastros)/property/PropertyCrud.tsx`, `frontend/src/dal/property/propertyApi.ts`, `frontend/src/domain/property/actions/`,
  - Documents: `frontend/src/domain/document/forms/FormDocument.tsx`, `frontend/src/app/(admin)/(others-pages)/(cadastros)/document/DocumentCrud.tsx`, `frontend/src/dal/document/documentApi.ts`, `frontend/src/domain/document/actions/`.
