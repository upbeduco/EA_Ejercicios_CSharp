# Abstract Data Types (ADTs) in C#

## What is an Abstract Data Type?

An **Abstract Data Type (ADT)** is a mathematical model that defines data *by what it does* rather than *how it's stored*. Think of it as a contract: "I promise you can add items, remove items, and check if it's empty" - but I don't tell you whether I'm using an array, linked list, or binary tree under the hood.

**Key principle:** Separate the *interface* (what operations are available) from the *implementation* (how those operations work internally).

## Core Design Principles for ADTs in C#

### 1. Interface-First Design

Always define your ADT as an interface first:

```csharp
public interface IStack<T>
{
    void Push(T item);
    T Pop();
    T Peek();
    bool IsEmpty { get; }
    int Count { get; }
}
```

**Why?** This allows you to swap implementations (array-based vs. linked-list-based) without changing client code.

### 2. Choose the Right Data Structure

| Use Case | Recommended Structure |
|----------|----------------------|
| **Data transfer (DTOs)** | `record` or `record struct` |
| **Internal collections** | Concrete types (`List<T>`, `Dictionary<K,V>`) |
| **Public APIs** | Interfaces (`IEnumerable<T>`, `IReadOnlyList<T>`) |
| **High-performance scenarios** | `Span<T>`, struct enumerators |

### 3. Immutability for Data Objects

Modern C# (2026 standards) strongly favors **immutable data types** for DTOs and value objects:

```csharp
// ✅ Good: Immutable record
public record UserProfile(Guid Id, string Username, DateTime CreatedAt);

// ❌ Avoid: Mutable class for data transfer
public class UserProfile
{
    public Guid Id { get; set; }
    public string Username { get; set; }
}
```

**Benefits:**
- Thread-safe by default
- Prevents accidental modification in multi-layer architectures
- Value-based equality (two records with same data are equal)

## Implementing Classic ADTs

### Stack Example

```csharp
public class ArrayStack<T> : IStack<T>
{
    private T[] _items;
    private int _count;
    
    public ArrayStack(int capacity = 4)
    {
        _items = new T[capacity];
    }
    
    public void Push(T item)
    {
        if (_count == _items.Length)
            Resize(_items.Length * 2);
        _items[_count++] = item;
    }
    
    public T Pop()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Stack is empty");
        return _items[--_count];
    }
    
    public T Peek()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Stack is empty");
        return _items[_count - 1];
    }
    
    public bool IsEmpty => _count == 0;
    public int Count => _count;
    
    private void Resize(int newCapacity)
    {
        var newArray = new T[newCapacity];
        Array.Copy(_items, newArray, _count);
        _items = newArray;
    }
}
```

### Queue Example

```csharp
public class CircularQueue<T> : IQueue<T>
{
    private T[] _items;
    private int _head;  // Index of first element
    private int _tail;  // Index where next element goes
    private int _count;
    
    public CircularQueue(int capacity = 4)
    {
        _items = new T[capacity];
    }
    
    public void Enqueue(T item)
    {
        if (_count == _items.Length)
            Resize(_items.Length * 2);
            
        _items[_tail] = item;
        _tail = (_tail + 1) % _items.Length;  // Wrap around
        _count++;
    }
    
    public T Dequeue()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Queue is empty");
            
        T item = _items[_head];
        _head = (_head + 1) % _items.Length;  // Wrap around
        _count--;
        return item;
    }
    
    public bool IsEmpty => _count == 0;
    public int Count => _count;
}
```

---

## Iteration Patterns for Collections

Making your custom collections **iterable** is essential for integration with C#'s `foreach` loop, LINQ, and other language features. Understanding iteration patterns is critical for building professional-grade data structures.

### The Iteration Protocol

C# uses a two-interface system for iteration:

1. **IEnumerable\<T\>** - The collection itself (factory for iterators)
2. **IEnumerator\<T\>** - The iteration state (cursor through the collection)

```csharp
public interface IEnumerable<T>
{
    IEnumerator<T> GetEnumerator();
}

public interface IEnumerator<T> : IDisposable
{
    T Current { get; }
    bool MoveNext();
    void Reset();  // Usually not implemented
}
```

**Key insight:** The separation allows **multiple simultaneous iterations** over the same collection. Each call to `GetEnumerator()` creates a fresh, independent cursor.

### Manual Iterator Implementation

Here's how to make a Stack iterable using manual implementation:

```csharp
public class IterableStack<T> : IEnumerable<T>
{
    private T[] _items;
    private int _count;
    
    public IterableStack(int capacity = 4)
    {
        _items = new T[capacity];
    }
    
    public void Push(T item)
    {
        if (_count == _items.Length)
            Resize(_items.Length * 2);
        _items[_count++] = item;
    }
    
    public T Pop()
    {
        if (_count == 0)
            throw new InvalidOperationException("Stack is empty");
        return _items[--_count];
    }
    
    // Iteration support
    public IEnumerator<T> GetEnumerator()
    {
        return new StackEnumerator(this);
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    // Nested enumerator class
    private class StackEnumerator : IEnumerator<T>
    {
        private readonly IterableStack<T> _stack;
        private int _index;
        private readonly int _version;  // For fail-fast detection
        
        public StackEnumerator(IterableStack<T> stack)
        {
            _stack = stack;
            _index = stack._count;  // Start from top
            _version = stack._version;
        }
        
        public T Current
        {
            get
            {
                if (_index <= 0 || _index > _stack._count)
                    throw new InvalidOperationException();
                return _stack._items[_index - 1];
            }
        }
        
        object IEnumerator.Current => Current;
        
        public bool MoveNext()
        {
            if (_version != _stack._version)
                throw new InvalidOperationException(
                    "Collection was modified during enumeration");
            
            if (_index > 0)
            {
                _index--;
                return true;
            }
            return false;
        }
        
        public void Reset()
        {
            _index = _stack._count;
        }
        
        public void Dispose() { }
    }
    
    private int _version;  // Incremented on modifications
    
    private void Resize(int newCapacity)
    {
        var newArray = new T[newCapacity];
        Array.Copy(_items, newArray, _count);
        _items = newArray;
    }
}
```

**Now you can use it with foreach:**
```csharp
var stack = new IterableStack<int>();
stack.Push(1);
stack.Push(2);
stack.Push(3);

foreach (var item in stack)
{
    Console.WriteLine(item);  // Prints: 3, 2, 1
}
```

### Using `yield return` (The Modern Way)

The `yield` keyword allows the compiler to generate the iterator state machine automatically:

```csharp
public class ModernStack<T> : IEnumerable<T>
{
    private T[] _items;
    private int _count;
    
    public ModernStack(int capacity = 4)
    {
        _items = new T[capacity];
    }
    
    public void Push(T item)
    {
        if (_count == _items.Length)
            Resize(_items.Length * 2);
        _items[_count++] = item;
    }
    
    public T Pop()
    {
        if (_count == 0)
            throw new InvalidOperationException("Stack is empty");
        return _items[--_count];
    }
    
    // Iterator using yield - much simpler!
    public IEnumerator<T> GetEnumerator()
    {
        // Iterate from top to bottom
        for (int i = _count - 1; i >= 0; i--)
        {
            yield return _items[i];
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    private void Resize(int newCapacity)
    {
        var newArray = new T[newCapacity];
        Array.Copy(_items, newArray, _count);
        _items = newArray;
    }
}
```

**Advantages of `yield return`:**
- **Less code:** No need to write a separate enumerator class
- **Lazy evaluation:** Items are produced on-demand
- **State management:** Compiler handles the complex state machine
- **Exception safety:** Automatic cleanup via generated `Dispose()`

### High-Performance: Struct Enumerators

For performance-critical code, use a **struct enumerator** to avoid heap allocation:

```csharp
public class PerformantStack<T> : IEnumerable<T>
{
    private T[] _items;
    private int _count;
    
    public PerformantStack(int capacity = 4)
    {
        _items = new T[capacity];
    }
    
    public void Push(T item)
    {
        if (_count == _items.Length)
            Resize(_items.Length * 2);
        _items[_count++] = item;
    }
    
    public T Pop()
    {
        if (_count == 0)
            throw new InvalidOperationException("Stack is empty");
        return _items[--_count];
    }
    
    // Return struct enumerator directly
    public Enumerator GetEnumerator() => new Enumerator(this);
    
    // Also implement interface for polymorphism
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    // Struct enumerator - no heap allocation!
    public struct Enumerator : IEnumerator<T>
    {
        private readonly PerformantStack<T> _stack;
        private int _index;
        
        internal Enumerator(PerformantStack<T> stack)
        {
            _stack = stack;
            _index = stack._count;
        }
        
        public T Current => _stack._items[_index];
        
        object IEnumerator.Current => Current;
        
        public bool MoveNext()
        {
            if (_index > 0)
            {
                _index--;
                return true;
            }
            return false;
        }
        
        public void Reset()
        {
            _index = _stack._count;
        }
        
        public void Dispose() { }
    }
    
    private void Resize(int newCapacity)
    {
        var newArray = new T[newCapacity];
        Array.Copy(_items, newArray, _count);
        _items = newArray;
    }
}
```

**Performance comparison:**

| Iterator Type | Allocation | Speed | Use Case |
|--------------|------------|-------|----------|
| **Class enumerator** | Heap | Slower | Polymorphic scenarios |
| **yield return** | Heap | Slower | Simple logic, readability |
| **Struct enumerator** | Stack | **Fastest** | Hot paths, tight loops |

### Iteration Patterns for Different Collection Types

#### Linked List (Forward Iteration)

```csharp
public class LinkedList<T> : IEnumerable<T>
{
    private class Node
    {
        public T Value;
        public Node Next;
    }
    
    private Node _head;
    private int _count;
    
    public void AddFirst(T item)
    {
        var node = new Node { Value = item, Next = _head };
        _head = node;
        _count++;
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        var current = _head;
        while (current != null)
        {
            yield return current.Value;
            current = current.Next;
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
```

#### Binary Tree (In-Order Traversal)

```csharp
public class BinarySearchTree<T> : IEnumerable<T> where T : IComparable<T>
{
    private class Node
    {
        public T Value;
        public Node Left;
        public Node Right;
    }
    
    private Node _root;
    
    public void Insert(T value)
    {
        _root = InsertRecursive(_root, value);
    }
    
    private Node InsertRecursive(Node node, T value)
    {
        if (node == null)
            return new Node { Value = value };
            
        if (value.CompareTo(node.Value) < 0)
            node.Left = InsertRecursive(node.Left, value);
        else
            node.Right = InsertRecursive(node.Right, value);
            
        return node;
    }
    
    // In-order traversal (sorted output)
    public IEnumerator<T> GetEnumerator()
    {
        return InOrderTraversal(_root).GetEnumerator();
    }
    
    private IEnumerable<T> InOrderTraversal(Node node)
    {
        if (node != null)
        {
            // Visit left subtree
            foreach (var item in InOrderTraversal(node.Left))
                yield return item;
                
            // Visit current node
            yield return node.Value;
            
            // Visit right subtree
            foreach (var item in InOrderTraversal(node.Right))
                yield return item;
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
```

**Usage:**
```csharp
var tree = new BinarySearchTree<int>();
tree.Insert(5);
tree.Insert(3);
tree.Insert(7);
tree.Insert(1);
tree.Insert(9);

foreach (var value in tree)
{
    Console.WriteLine(value);  // Prints: 1, 3, 5, 7, 9 (sorted!)
}
```

### Multiple Iteration Modes

Sometimes you want different ways to traverse the same collection:

```csharp
public class FlexibleList<T> : IEnumerable<T>
{
    private T[] _items;
    private int _count;
    
    public FlexibleList(int capacity = 4)
    {
        _items = new T[capacity];
    }
    
    public void Add(T item)
    {
        if (_count == _items.Length)
            Resize(_items.Length * 2);
        _items[_count++] = item;
    }
    
    // Default: forward iteration
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _count; i++)
            yield return _items[i];
    }
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    // Alternative: reverse iteration
    public IEnumerable<T> Reverse()
    {
        for (int i = _count - 1; i >= 0; i--)
            yield return _items[i];
    }
    
    // Alternative: skip every other element
    public IEnumerable<T> EveryOther()
    {
        for (int i = 0; i < _count; i += 2)
            yield return _items[i];
    }
    
    // Alternative: batch processing
    public IEnumerable<T[]> Batch(int size)
    {
        for (int i = 0; i < _count; i += size)
        {
            int batchSize = Math.Min(size, _count - i);
            var batch = new T[batchSize];
            Array.Copy(_items, i, batch, 0, batchSize);
            yield return batch;
        }
    }
    
    private void Resize(int newCapacity)
    {
        var newArray = new T[newCapacity];
        Array.Copy(_items, newArray, _count);
        _items = newArray;
    }
}
```

**Usage:**
```csharp
var list = new FlexibleList<int>();
for (int i = 1; i <= 10; i++)
    list.Add(i);

// Forward: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
foreach (var item in list)
    Console.Write($"{item} ");

// Reverse: 10, 9, 8, 7, 6, 5, 4, 3, 2, 1
foreach (var item in list.Reverse())
    Console.Write($"{item} ");

// Every other: 1, 3, 5, 7, 9
foreach (var item in list.EveryOther())
    Console.Write($"{item} ");

// Batches of 3: [1,2,3], [4,5,6], [7,8,9], [10]
foreach (var batch in list.Batch(3))
    Console.WriteLine($"[{string.Join(",", batch)}]");
```

### Thread Safety and Iteration

**⚠️ Warning:** By default, iterating over a collection while another thread modifies it is **not safe**.

```csharp
public class ThreadSafeStack<T>
{
    private readonly object _lock = new object();
    private T[] _items;
    private int _count;
    
    public ThreadSafeStack(int capacity = 4)
    {
        _items = new T[capacity];
    }
    
    public void Push(T item)
    {
        lock (_lock)
        {
            if (_count == _items.Length)
                Resize(_items.Length * 2);
            _items[_count++] = item;
        }
    }
    
    public T Pop()
    {
        lock (_lock)
        {
            if (_count == 0)
                throw new InvalidOperationException("Stack is empty");
            return _items[--_count];
        }
    }
    
    // Safe iteration: take a snapshot
    public IEnumerable<T> GetSnapshot()
    {
        T[] snapshot;
        lock (_lock)
        {
            snapshot = new T[_count];
            Array.Copy(_items, snapshot, _count);
        }
        
        // Iterate over the snapshot (outside the lock)
        for (int i = _count - 1; i >= 0; i--)
            yield return snapshot[i];
    }
    
    private void Resize(int newCapacity)
    {
        var newArray = new T[newCapacity];
        Array.Copy(_items, newArray, _count);
        _items = newArray;
    }
}
```

### Lazy Evaluation with `yield`

One of the most powerful features of `yield` is **deferred execution**:

```csharp
public static class MathSequences
{
    // Generates Fibonacci numbers on-demand
    public static IEnumerable<int> Fibonacci()
    {
        int a = 0, b = 1;
        while (true)  // Infinite sequence!
        {
            yield return a;
            (a, b) = (b, a + b);
        }
    }
    
    // Filters only prime numbers
    public static IEnumerable<int> Primes()
    {
        yield return 2;
        
        for (int candidate = 3; ; candidate += 2)
        {
            bool isPrime = true;
            for (int divisor = 3; divisor * divisor <= candidate; divisor += 2)
            {
                if (candidate % divisor == 0)
                {
                    isPrime = false;
                    break;
                }
            }
            
            if (isPrime)
                yield return candidate;
        }
    }
}
```

**Usage:**
```csharp
// Get first 10 Fibonacci numbers
var firstTen = MathSequences.Fibonacci().Take(10);
foreach (var num in firstTen)
    Console.Write($"{num} ");  // 0 1 1 2 3 5 8 13 21 34

// Get first 5 primes greater than 100
var primes = MathSequences.Primes()
    .SkipWhile(p => p <= 100)
    .Take(5);
foreach (var prime in primes)
    Console.Write($"{prime} ");  // 101 103 107 109 113
```

### Common Pitfalls with Iterators

#### Pitfall 1: Multiple Enumeration

```csharp
// ❌ BAD: Expensive operation executed twice
var data = GetExpensiveData().Where(x => x.IsValid);
int count = data.Count();        // First enumeration
var first = data.FirstOrDefault(); // Second enumeration

// ✅ GOOD: Materialize once
var data = GetExpensiveData().Where(x => x.IsValid).ToList();
int count = data.Count;          // Fast: already in memory
var first = data.FirstOrDefault();
```

#### Pitfall 2: Modifying During Iteration

```csharp
var list = new List<int> { 1, 2, 3, 4, 5 };

// ❌ BAD: Throws InvalidOperationException
foreach (var item in list)
{
    if (item % 2 == 0)
        list.Remove(item);  // Modifies collection during iteration!
}

// ✅ GOOD: Iterate over a copy
foreach (var item in list.ToList())
{
    if (item % 2 == 0)
        list.Remove(item);
}

// ✅ BETTER: Use RemoveAll
list.RemoveAll(x => x % 2 == 0);
```

#### Pitfall 3: Forgetting Disposal

```csharp
// ❌ BAD: File handle might leak if exception occurs
IEnumerable<string> ReadLines(string path)
{
    var reader = new StreamReader(path);
    string line;
    while ((line = reader.ReadLine()) != null)
        yield return line;
    reader.Dispose();  // May never reach here!
}

// ✅ GOOD: Use 'using' - compiler handles cleanup
IEnumerable<string> ReadLines(string path)
{
    using var reader = new StreamReader(path);
    string line;
    while ((line = reader.ReadLine()) != null)
        yield return line;
}
```

---

## ADTs as DTOs (Data Transfer Objects)

### REST API Example

```csharp
// API Request DTO
public record CreateOrderRequest(
    [Required] Guid CustomerId,
    [Required] [MinLength(1)] List<OrderItem> Items,
    string? PromotionCode
);

// API Response DTO
public record OrderResponse(
    Guid OrderId,
    decimal TotalAmount,
    OrderStatus Status,
    DateTime CreatedAt
);

// API Controller
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    [HttpPost]
    public ActionResult<OrderResponse> CreateOrder(CreateOrderRequest request)
    {
        // Validation happens automatically via attributes
        // Convert DTO to domain model, process, convert back
        var response = _orderService.CreateOrder(request);
        return Ok(response);
    }
}
```

**Key practices:**
- Use `record` for automatic value equality
- Add validation attributes (`[Required]`, `[Range]`, etc.)
- Make optional fields nullable (`string?`)
- Name clearly: `CreateOrderRequest` not `OrderDTO`

### Backend-for-Frontend (BFF) Pattern

```csharp
// Internal microservice DTOs (what backend returns)
public record UserServiceResponse(Guid Id, string Email, string Name);
public record OrderServiceResponse(Guid OrderId, decimal Total);

// BFF-specific DTO (what mobile app needs)
public record MobileUserDashboard(
    string DisplayName,           // Computed from Name
    int TotalOrders,             // Aggregated count
    decimal LifetimeSpending,    // Aggregated sum
    string? LastOrderStatus      // Latest order only
);

// BFF Controller aggregates and transforms
public class BFFController : ControllerBase
{
    [HttpGet("dashboard")]
    public async Task<MobileUserDashboard> GetDashboard(Guid userId)
    {
        // Call multiple services
        var user = await _userService.GetUser(userId);
        var orders = await _orderService.GetUserOrders(userId);
        
        // Transform into UI-specific shape
        return new MobileUserDashboard(
            DisplayName: user.Name.Split(' ')[0],  // First name only
            TotalOrders: orders.Count,
            LifetimeSpending: orders.Sum(o => o.Total),
            LastOrderStatus: orders.OrderByDescending(o => o.CreatedAt)
                                  .FirstOrDefault()?.Status
        );
    }
}
```

### Message Queue Example

```csharp
// Message contract (shared between producer and consumer)
public interface IOrderSubmittedEvent
{
    Guid OrderId { get; }
    Guid CorrelationId { get; }  // For idempotency
    DateTime OccurredAt { get; }
}

// Implementation (in shared contracts library)
public record OrderSubmittedEvent(
    Guid OrderId,
    Guid CorrelationId,
    DateTime OccurredAt
) : IOrderSubmittedEvent;

// Consumer
public class OrderNotificationConsumer : IConsumer<IOrderSubmittedEvent>
{
    public async Task Consume(ConsumeContext<IOrderSubmittedEvent> context)
    {
        // Process the message
        var orderId = context.Message.OrderId;
        await _emailService.SendConfirmation(orderId);
    }
}
```

**Message queue best practices:**
- Use `interface` definitions for loose coupling
- Always include `CorrelationId` for duplicate detection
- Make messages immutable (records are perfect)
- Put contracts in a shared library

## Modern C# Features for ADTs

### Discriminated Unions (Pattern Matching)

```csharp
public abstract record Result;
public sealed record Success(decimal NewBalance) : Result;
public sealed record Failure(string ErrorMessage) : Result;

public void ProcessTransaction(Result result)
{
    // Exhaustive pattern matching
    switch (result)
    {
        case Success(var balance):
            Console.WriteLine($"Success! New balance: {balance}");
            break;
        case Failure(var error):
            Console.WriteLine($"Failed: {error}");
            break;
    }
}
```

### High-Performance with Span<T>

```csharp
public class HighPerformanceStack<T>
{
    private T[] _items;
    
    // Return span for zero-copy access
    public Span<T> AsSpan() => _items.AsSpan(0, Count);
    
    // Process without allocation
    public void ProcessAll(Action<T> action)
    {
        var span = AsSpan();
        foreach (ref readonly var item in span)
        {
            action(item);
        }
    }
}
```

## Common Pitfalls to Avoid

### ❌ Don't: Return Internal Collections Directly

```csharp
public class BadDesign
{
    private List<int> _items = new();
    
    // Client can modify internal state!
    public List<int> GetItems() => _items;
}
```

### ✅ Do: Return Read-Only Views

```csharp
public class GoodDesign
{
    private List<int> _items = new();
    
    // Client cannot modify internal state
    public IReadOnlyList<int> GetItems() => _items.AsReadOnly();
}
```

### ❌ Don't: Use Classes for Simple Data

```csharp
public class Point  // Requires heap allocation
{
    public int X { get; set; }
    public int Y { get; set; }
}
```

### ✅ Do: Use Records or Structs

```csharp
public record struct Point(int X, int Y);  // Stack-allocated, value equality
```

## Summary Checklist

When implementing ADTs in C#:

- [ ] Define interface first (what operations?)
- [ ] Choose appropriate type (`record` for data, `class` for behavior)
- [ ] Make data immutable when possible
- [ ] Use validation attributes for DTOs
- [ ] Return interfaces from public methods
- [ ] Include `CorrelationId` in message contracts
- [ ] Consider performance (struct enumerators, spans for hot paths)
- [ ] Document time complexity of operations
- [ ] Implement `IEnumerable<T>` for custom collections
- [ ] Use `yield return` for simple iteration logic
- [ ] Consider struct enumerators for performance-critical paths
- [ ] Provide multiple iteration modes when useful

## Iteration Best Practices Summary

| Scenario | Recommendation | Reason |
|----------|---------------|---------|
| **Simple iteration** | Use `yield return` | Clean, maintainable code |
| **Performance-critical** | Struct enumerator | Zero allocation |
| **Multiple cursors** | Separate enumerator class | Independent state |
| **Infinite sequences** | `yield return` with `while(true)` | Lazy evaluation |
| **Resource management** | `yield` with `using` | Automatic cleanup |
| **Thread-safe iteration** | Take snapshot first | Prevent modification during iteration |

## Further Practice

Try implementing these ADTs with full iteration support:

1. **Priority Queue** with a binary heap - iterate in priority order
2. **Doubly Linked List** - support both forward and reverse iteration
3. **Hash Table** - iterate over key-value pairs
4. **Binary Search Tree** - provide in-order, pre-order, and post-order traversals
5. **Graph** - implement BFS and DFS as `IEnumerable<T>` methods

Remember: The goal is not just to "make it work" but to design a **clean interface** that hides implementation details, allows for future optimization, and integrates seamlessly with C#'s language features like `foreach`, LINQ, and pattern matching.
