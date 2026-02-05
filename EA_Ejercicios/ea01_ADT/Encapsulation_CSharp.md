# Encapsulation in C#

## Introduction

**Encapsulation** is one of the four pillars of object-oriented programming (alongside abstraction, inheritance, and polymorphism). It is the practice of bundling data (fields) and behavior (methods) together while hiding the internal details from the outside world. In the context of data structures and algorithms, encapsulation ensures that your collections and abstract data types (ADTs) are **robust**, **maintainable**, and **correct**.

### Why Encapsulation Matters in DSA

When you implement a data structure like a stack, queue, or binary search tree, you want to guarantee that:
- The structure is always in a valid state
- External code cannot accidentally corrupt the internal representation
- The implementation can be changed without breaking client code
- The API is clear and intuitive

Encapsulation achieves all of this.

---

## Core Principle 1: Minimize Visibility (Information Hiding)

### The Rule

- **Instance fields should be `private`** (or at minimum, non-public).
- Expose only the methods that clients *need*, not what is convenient to expose.
- Default to the **narrowest possible access level**: `private` → `protected` → `internal` → `public`.

### Why It Matters

Consider a simple `Stack<T>` implementation:

```csharp
// ❌ BAD: Exposing internal structure
public class BadStack<T>
{
    public T[] elements;  // Public field—anyone can modify!
    public int count;     // Public count—anyone can change it!

    public void Push(T item)
    {
        elements[count++] = item;
    }

    public T Pop()
    {
        return elements[--count];
    }
}
```

**Problems:**
- External code can set `count = -1` or `count = 1000`, breaking the stack's state.
- If you later decide to use a `List<T>` internally instead of an array, all external code breaks.
- There's no validation—someone could set `count` to an invalid value.

```csharp
// ✅ GOOD: Hiding internal structure
public class GoodStack<T>
{
    private List<T> elements;  // Private—hidden from clients

    public void Push(T item)
    {
        elements.Add(item);
    }

    public T Pop()
    {
        if (elements.Count == 0)
            throw new InvalidOperationException("Stack is empty");
        
        T item = elements[elements.Count - 1];
        elements.RemoveAt(elements.Count - 1);
        return item;
    }

    public int Count => elements.Count;  // Expose only what's needed

    public bool IsEmpty => elements.Count == 0;
}
```

**Benefits:**
- Internal `List<T>` is protected; clients cannot corrupt it.
- Can change the implementation (e.g., to an array) without affecting client code.
- Methods validate state (e.g., check if stack is empty before popping).
- Clear, safe API.

---

## Core Principle 2: Maintain Class Invariants

### What Is an Invariant?

An **invariant** is a property that must *always* be true for an object to be in a valid state. For example:
- In a `Stack<T>`: the internal count should match the actual number of elements.
- In a `MinHeap`: every parent should be smaller than its children.
- In a `Dictionary`: each key should appear only once.

### Enforcing Invariants

Encapsulation allows you to enforce invariants by controlling how the object's state changes.

```csharp
public class BankAccount
{
    private decimal balance;  // Invariant: balance >= 0

    public BankAccount(decimal initialBalance)
    {
        if (initialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative");
        balance = initialBalance;
    }

    public void Withdraw(decimal amount)
    {
        // Validate before changing state
        if (amount < 0)
            throw new ArgumentException("Withdrawal amount cannot be negative");
        if (amount > balance)
            throw new InvalidOperationException("Insufficient funds");
        
        balance -= amount;  // Now it's safe to mutate
    }

    public decimal GetBalance() => balance;  // Read-only access
}
```

**Key insight:** By making `balance` private and only providing controlled access through `Withdraw()`, you ensure the invariant (non-negative balance) is never violated.

---

## Core Principle 3: Prefer Immutability

### Immutable Objects Are Naturally Encapsulated

An immutable object is one that cannot be changed after creation. Immutability eliminates entire classes of bugs:
- Thread-safe by design
- Invariants are guaranteed once the constructor completes
- No need to worry about defensive copies

### Using `record` and `readonly` in C#

C# 9+ introduced **records**, which are perfect for immutable ADTs:

```csharp
// ✅ Immutable record—clean and safe
public record Point(double X, double Y);

// Usage
var p1 = new Point(3, 4);
var p2 = new Point(3, 4);
Console.WriteLine(p1 == p2);  // true (value equality, not reference equality!)
```

For classes, use `readonly` fields and `init` properties:

```csharp
public class Node<T>
{
    public T Value { get; init; }           // Can only be set in constructor
    public Node<T> Next { get; private set; }  // Private setter for controlled mutation

    public Node(T value)
    {
        Value = value;
        Next = null;
    }
}
```

### Benefits for Data Structures

Immutability is especially useful for nodes in linked structures:

```csharp
public class LinkedList<T>
{
    // Node is effectively immutable (once created, cannot change)
    private sealed class Node
    {
        public T Value { get; }
        public Node Next { get; set; }

        public Node(T value, Node next = null)
        {
            Value = value;
            Next = next;
        }
    }

    private Node head;

    public void PushFront(T value)
    {
        head = new Node(value, head);  // Create new node; don't modify existing ones
    }
}
```

---

## Core Principle 4: Avoid "Anemic" Getters and Setters

### The Problem

Not every field needs a getter and setter. Blind use of auto-properties turns objects into data containers without behavior:

```csharp
// ❌ BAD: An anemic class with no real behavior
public class BankAccount
{
    public decimal Balance { get; set; }  // Anyone can set to any value!
    public string Owner { get; set; }
    public DateTime OpenedDate { get; set; }
}

// Client code
var account = new BankAccount();
account.Balance = -1000;  // Oops! Invariant violated.
```

### The Solution: Expose Behavior, Not Data

```csharp
// ✅ GOOD: Rich domain object with clear intent
public class BankAccount
{
    private decimal balance;
    public string Owner { get; }
    public DateTime OpenedDate { get; }

    public BankAccount(string owner, decimal initialBalance = 0)
    {
        if (string.IsNullOrWhiteSpace(owner))
            throw new ArgumentException("Owner name is required");
        if (initialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative");

        Owner = owner;
        balance = initialBalance;
        OpenedDate = DateTime.Now;
    }

    // Expose behavior, not raw data
    public decimal GetBalance() => balance;

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be positive");
        balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be positive");
        if (amount > balance)
            throw new InvalidOperationException("Insufficient funds");
        balance -= amount;
    }

    // More informative methods
    public bool CanWithdraw(decimal amount) => amount > 0 && amount <= balance;
    public bool IsEmpty() => balance == 0;
}
```

**Key difference:** Instead of `Balance { get; set; }`, we provide domain-specific methods: `Deposit()`, `Withdraw()`, `GetBalance()`. This makes intent clear and prevents invalid state changes.

---

## Core Principle 5: Validate Inputs at the Boundary

### Fail Fast and Clearly

Validation should happen as early as possible—preferably in the constructor:

```csharp
// ✅ GOOD: Validate in constructor (fail-fast approach)
public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
{
    private List<(TElement element, TPriority priority)> items;

    public PriorityQueue(int capacity = 16)
    {
        if (capacity < 1)
            throw new ArgumentException("Capacity must be at least 1", nameof(capacity));
        
        items = new List<(TElement, TPriority)>(capacity);
    }

    public void Enqueue(TElement element, TPriority priority)
    {
        if (element == null)
            throw new ArgumentNullException(nameof(element));
        if (priority == null)
            throw new ArgumentNullException(nameof(priority));

        // Now we know it's safe to proceed
        items.Add((element, priority));
        // ... bubble-up logic ...
    }
}
```

**Benefits:**
- Invalid objects are rejected immediately.
- Clearer error messages for debugging.
- Rest of the code can assume valid inputs.

---

## Core Principle 6: Control Mutability of Exposed State

### Never Leak Mutable References

If you expose a mutable collection or object, external code can corrupt it:

```csharp
// ❌ BAD: Direct reference to internal collection
public class StudentGroup
{
    private List<string> students = new();

    public List<string> GetStudents() => students;  // DANGER!
}

var group = new StudentGroup();
group.GetStudents().Add("Eve");      // Sneaky modification!
group.GetStudents().Clear();         // Or even: delete everyone!
```

### The Solution: Return Read-Only Views or Copies

```csharp
// ✅ GOOD: Return read-only interface
public class StudentGroup
{
    private List<string> students = new();

    public IReadOnlyList<string> GetStudents() => students.AsReadOnly();
    
    public void AddStudent(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name required");
        students.Add(name);
    }
}

var group = new StudentGroup();
var readOnly = group.GetStudents();
// readOnly.Add("Eve");  // Compile error! IReadOnlyList has no Add method
```

---

## Core Principle 7: Separate Internal Representation from Public API

### Hide Implementation Details

The internal structure should be invisible to clients. This allows you to refactor without breaking code:

```csharp
// Example: Hash table for a dictionary

// ❌ BAD: Public API reveals internal structure
public class SimpleDictionary<K, V>
{
    public KeyValuePair<K, V>[] buckets;  // Exposed!
    public int count;                     // Exposed!
}

// ✅ GOOD: Internal representation is hidden
public class SimpleDictionary<K, V>
{
    private KeyValuePair<K, V>[] buckets;
    private int count;

    // Public API doesn't reveal how we store data
    public void Add(K key, V value) { /* ... */ }
    public bool TryGetValue(K key, out V value) { /* ... */ }
    public int Count => count;
}
```

**Future-proof:** If you later decide to use a tree-based structure instead of a hash table, client code doesn't care—the public API is unchanged.

---

## Core Principle 8: Use Constructors and Factories Intentionally

### Enforce Valid Creation Paths

Not all configurations are valid. Use constructors and factory methods to ensure objects start in a valid state:

```csharp
// ❌ BAD: No way to guarantee valid state
public class TreeNode<T>
{
    public T Value { get; set; }
    public TreeNode<T> Left { get; set; }
    public TreeNode<T> Right { get; set; }

    public TreeNode() { }  // Empty constructor—no invariants enforced
}

// ✅ GOOD: Constructor enforces invariants
public class TreeNode<T>
{
    public T Value { get; }
    public TreeNode<T> Left { get; set; }
    public TreeNode<T> Right { get; set; }

    // Must provide a value at creation time
    public TreeNode(T value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));
        Value = value;
    }

    // Factory method for more readable creation
    public static TreeNode<T> CreateLeaf(T value) => new TreeNode<T>(value);
}
```

---

## Core Principle 9: The Law of Demeter (Tell, Don't Ask)

### Don't Reach Through Objects

Avoid chains of method calls that reach into the internals of other objects:

```csharp
// ❌ BAD: Reaching through objects (violates Law of Demeter)
var city = student.GetAddress().GetCity();
student.GetAddress().GetCity().IncrementPopulation();

// ✅ GOOD: Ask the object to do the work
var city = student.GetCityName();
student.IncrementCityPopulation();
```

**Why it matters:** If `Address` or `City` changes its structure, all code that reaches through them breaks. Encapsulation is violated across the entire chain.

---

## Core Principle 10: Composition Over Inheritance

### Inheritance Breaks Encapsulation

When you inherit from a class, you gain access to `protected` members, which exposes implementation details:

```csharp
// ❌ RISKY: Inheritance exposes protected internals
public class Stack<T>
{
    protected List<T> elements;  // Protected—subclasses can see this!
}

public class InstrumentedStack<T> : Stack<T>
{
    public void Push(T item)
    {
        // Subclass can directly access protected field
        elements.Add(item);  // Bypasses any logic in base class!
    }
}
```

**Problem:** If the base class changes how `elements` is managed, the subclass may break silently.

```csharp
// ✅ BETTER: Composition and delegation
public class InstrumentedStack<T>
{
    private Stack<T> stack;  // Compose instead of inherit

    public void Push(T item)
    {
        stack.Push(item);  // Delegate to the composed object
    }

    public T Pop() => stack.Pop();

    public int Count => stack.Count;
}
```

**Benefit:** No exposure of internals. If `Stack` changes, `InstrumentedStack` continues to work because it only depends on the public API.

---

## Access Modifiers in C#: A Quick Reference

| Modifier | Visible From | Use Case |
|----------|--------------|----------|
| `private` | Same class only | Default for fields; most restrictive |
| `protected` | Same class and derived classes | Base class behavior for subclasses |
| `internal` | Same assembly only | Hide implementation from other assemblies |
| `public` | Anywhere | Public API; be deliberate |

**Best practice:** Start with `private`, then increase visibility only when needed.

---

## Practical Example: Building a Sorted List

Let's apply all principles to a real ADT:

```csharp
/// <summary>
/// A sorted list maintains elements in ascending order.
/// Invariants: List is always sorted; no duplicates allowed.
/// </summary>
public class SortedList<T> where T : IComparable<T>
{
    private List<T> items;  // Private—hidden

    public int Count => items.Count;  // Public property: read-only
    public IReadOnlyList<T> Items => items.AsReadOnly();  // Safe view

    // Constructor enforces invariants
    public SortedList()
    {
        items = new List<T>();
    }

    // Public method: express behavior, not data manipulation
    public void Add(T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        // Maintain invariant: no duplicates
        if (Contains(item))
            throw new InvalidOperationException("Item already exists");

        // Find correct position and insert
        int index = items.BinarySearch(item);
        if (index < 0) index = ~index;  // Bitwise complement gives insertion point
        items.Insert(index, item);
    }

    // Encapsulated lookup
    public bool Contains(T item)
    {
        if (item == null) return false;
        return items.BinarySearch(item) >= 0;
    }

    // Encapsulated removal
    public bool Remove(T item)
    {
        if (item == null) return false;
        
        int index = items.BinarySearch(item);
        if (index >= 0)
        {
            items.RemoveAt(index);
            return true;
        }
        return false;
    }

    // Invariant validation (for debugging)
    private void ValidateInvariant()
    {
        for (int i = 1; i < items.Count; i++)
        {
            if (items[i].CompareTo(items[i - 1]) <= 0)
                throw new InvalidOperationException("Invariant violated: list is not sorted");
        }
    }
}
```

**Key features:**
- Private `items` field—hidden from clients
- Public methods (`Add`, `Remove`, `Contains`)—express intent
- Input validation—enforce pre-conditions
- Invariant maintenance—list stays sorted
- Safe exposure—`Items` returns `IReadOnlyList<T>`

---

## Common Pitfalls to Avoid

### Pitfall 1: Over-Exposing with Properties

```csharp
// ❌ BAD: Auto-properties expose everything
public class Node<T>
{
    public T Value { get; set; }
    public Node<T> Next { get; set; }
}

// ✅ GOOD: Controlled access
public class Node<T>
{
    public T Value { get; }
    public Node<T> Next { get; private set; }

    public Node(T value)
    {
        Value = value;
    }
}
```

### Pitfall 2: Forgetting to Validate

```csharp
// ❌ BAD: No validation in constructor
public class Queue<T>
{
    public Queue(int capacity) { }
}

new Queue<int>(-5);  // Invalid! But no one stops you.

// ✅ GOOD: Validate early
public class Queue<T>
{
    public Queue(int capacity)
    {
        if (capacity < 1)
            throw new ArgumentException("Capacity must be positive");
    }
}
```

### Pitfall 3: Exposing Mutable Collections

```csharp
// ❌ BAD
public class Graph<T>
{
    public List<T> Vertices => vertices;  // Mutable!
}

graph.Vertices.Clear();  // Oh no!

// ✅ GOOD
public class Graph<T>
{
    public IReadOnlyList<T> Vertices => vertices.AsReadOnly();
}
```

---

## Summary: The Encapsulation Checklist

When designing a new class or ADT, ask yourself:

- [ ] Are all fields `private`?
- [ ] Do methods validate their inputs?
- [ ] Are invariants clearly documented?
- [ ] Are invariants maintained by all public methods?
- [ ] Does the public API express *behavior* rather than expose *data*?
- [ ] Are mutable collections wrapped in read-only views?
- [ ] Is the constructor used to enforce valid initialization?
- [ ] Could a future maintainer change the internal implementation without breaking client code?

If you answer "yes" to all of these, you have good encapsulation!

---

## Further Reading

- **Framework Design Guidelines** (Microsoft): Official guidance on API design in C#
- **Code Complete** by Steve McConnell: Classic reference on encapsulation and defensive programming
- **Domain-Driven Design** by Eric Evans: How encapsulation supports domain modeling

---

**Remember:** Encapsulation is not about making your code harder to use—it's about making it *safer*, *clearer*, and *more maintainable*. Invest in good encapsulation today, and your future self (and your teammates) will thank you!
