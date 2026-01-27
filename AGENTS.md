# Examples and exercises about Data Structures and Algorithms

This is an educational set of examples and exercises in C# about data structures and algorithms based on the Sedgewick "Algorithms, 4th Edition" textbook.

## General guidelines

- When implementing ADTs, always ensure proper encapsulation.
- ADTs should provide basic tests of their methods in their own `Main` method using `Debug.Assert()`. Do not use other testing frameworks. Each ADT's `Main` method should include at least 3-5 assert statements covering normal cases, edge cases (e.g., empty structures), and error conditions.
- To test expected exceptions, use the following pattern:

  ```csharp
  try {
      stack.Pop(); // empty stack
      Debug.Assert(false, "Should have thrown exception");
  } catch (InvalidOperationException) {
      // expected
  }
  ```

- Classes and methods should include XML documentation comments (`///`).
- ADTs should not accept `null` as a valid element; throw `ArgumentNullException` if `null` is passed.

## Code Style and Conventions

- Follow standard C# naming conventions: classes, methods, and properties in PascalCase (e.g., `ListaSimple`, `AddHead`, `IsEmpty`), private fields in camelCase or with underscore prefix (e.g., `_count`, `first`), constants in PascalCase.
- Use consistent indentation (4 spaces, no tabs) and adhere to a 120-character line limit for readability.
- Organize `using` directives alphabetically at the top of the file. Prefer explicit namespace imports over global usings for clarity.
- Implement ADTs using generics where appropriate (e.g., `class ListaSimple<T>` for type safety), but keep implementations simple for educational purposes unless specified in exercises.
- **Language policy**: namespace and class names may use Spanish (e.g., `ListaSimple`, `Fecha`). Method names should use English (e.g., `AddHead`, `RemoveHead`). XML doc comments and inline comments may use Spanish.

## Project Structure and Dependencies

- Maintain the folder structure under the `EA_Ejercicios` project (e.g., `ea01_ADT/`, `ea02_EstructurasBasicas/`) for all new classes, using the `EA_UPB` namespace. Ensure new files align with the existing folder hierarchy.
- Avoid external NuGet packages unless explicitly required by an exercise; rely on standard .NET APIs to keep the project self-contained and focused on core concepts.
- The `StdLib` project provides I/O utilities (`StdIn`, `StdRandom`, `Scanner`, `In`) and the `Algs4` project provides reference algorithm implementations. Use them for I/O and utilities; prefer custom implementations over Algs4 for ADTs and algorithms that are the subject of an exercise.
- To build from the command line: `dotnet build EA_Ejercicios.sln`
- To run a specific project: `dotnet run --project EA_Ejercicios`

## Implementation Best Practices

- Prioritize efficiency and correctness in algorithms: document time/space complexity in XML doc comments using the format `Time: O(n), Space: O(1)`, especially in modules like ea03_AnalisisDeAlgoritmos or ea04_MetodosDeOrdenacion.
- Handle edge cases and exceptions gracefully (e.g., use `ArgumentException` for invalid inputs, `InvalidOperationException` for illegal state in ADTs like stacks/queues), but keep error handling minimal for exercise simplicity.
- Value-type ADTs (e.g., `Fecha`, `Punto2D`) should be immutable: declare fields as `readonly`, use `init` or read-only properties with no setters, and return defensive copies from accessors when needed. Container ADTs (e.g., `Pila`, `Cola`, `ListaSimple`) are inherently mutable but should protect internal state from external modification.
- Always override `ToString()` in ADTs for debugging and educational output.
- Override `Equals()` and `GetHashCode()` in value-type ADTs (e.g., `Fecha`, `Punto2D`). Follow the standard equality contract: reflexive, symmetric, transitive, consistent, and null-safe (return `false` for `null`). Consider implementing `IEquatable<T>` for type-safe equality.
- Implement `IComparable<T>` for ADTs with a natural ordering (e.g., `Fecha`). Provide separate `IComparer<T>` implementations for alternate orderings (e.g., sorting by different fields).
- All collection ADTs should implement `IEnumerable<T>` with a private inner enumerator class.
- Use private nested classes for internal nodes (`Nodo`) in linked data structures.

## Documentation and Collaboration

- Enhance XML documentation: include `<param>`, `<returns>`, and `<exception>` tags for all public methods, plus brief examples in `<example>` or `<code>` blocks. For classes, add a `<summary>` with a high-level description of the ADT's purpose and invariants.
- When proposing code changes, reference relevant exercises (e.g., from [ea02_EstructurasBasicas/EXERCISES.md](ea02_EstructurasBasicas/EXERCISES.md)) and explain algorithmic choices to aid learning.
- If generating new files or features, ensure they fit the project's educational scope—focus on fundamental data structures without over-engineering.
