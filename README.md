# How to Use

1. **Dump the Game**:
   - Use **Unispect** to dump the game data, I use DMA plugin for this from DER discord.
   - Save the dump file for processing.

2. **Prepare the Dumper**:
   - Open source code in your favorite editor, define the path variables in EFTProcessor.cs file.
   - Put dump file into directory as defined in file EFTProcessor.cs in variable DUMP_INPUT_PATH
   - Put Assembly-CSharp.dll file into directory as defined in file EFTProcessor.cs in variable ASSEMBLY_INPUT_PATH
3. **Run Dumper**
   - Should create the sdk.cs file in the path you've previously set.

## Requirements
- **Unispect** for initial game dumping.


# Docs

## Terminology I use to explain

- Anchor: A field, method, class name, or other lexical token you search for to find the desired structure in the dump file or in assembly-csharp. https://en.wikipedia.org/wiki/Lexical_analysis#Token

- Scope: The range a variable can be referenced. https://en.wikipedia.org/wiki/Scope_(computer_science)


## ProcessClassNames function

If you want to find methods of classes, add your structures in scope of this function.

### entity
Anchor, field or class name usually.

### variable
Dumper equals it to the real class name in Assembly-CSharp in SDK.cs ("ClassName" is typically used for field name)

### SetVariableStatus(variable);
Resets the finder module to target our anchor (entity).

### new StructureGenerator object
The parameter is the name you want the struct in sdk.cs

### nestedStruct.AddClassName() 
Should be first, it actually uses the finder module to get the method defined in string entity.

### nestedStruct.AddString() 
Add a string type field to the struct, defining the exact method name to target.

### structGenerator.AddStruct(nestedStruct)  
ProcessClassNames parameter object to add our nested structure to the main file.


## ProcessOffsets

If you want fields of a class.

### TypeDef name_here = default;
Variable in code that holds the return value from _dnlibHelper.FindClassWithEntityName()

### name
Name of the struct that gets written into SDK.cs

### SetVariableStatus(name); 
Resets the finder module to target our anchor. Parameter is the name variabe defined above.

### new StructureGenerator object
The parameter is the name variable defined above.

### const string ClassName
The actual class name from Assembly-CSharp.

### entity
Field name you need the offset of

### set the TypeDef variable from step 1 to the return of _dnlibHelper.FindClassWithEntityName()

### var offset
dynamic typed variable set to result of _dumpParser.FindOffsetByTypeName(ClassName, $"-.{MenuOperationClass.Humanize()}");

### call to nestedStruct.AddOffset(entity, offset);

### call to structGenerator.AddStruct(nestedStruct);

## Referenced fields, classes, methods, object instances - HOW TO - written by Trae AI IDE helper.

**Starting Point: The Desired Information (`Delay` in `AfkMonitor`)**
    The goal was to find the `Delay` value within the `AfkMonitor`. So, the first step would be to locate the `AfkMonitor` class itself. This might be done by searching for strings related to "AFK" or by observing its behavior in the game and then looking for code that controls that behavior.

2.  **Locating `AfkMonitor` and its `Delay` Field/Property**
    Once `AfkMonitor` is found (let's say it's `GClassX`), the next step is to examine its members. The `Delay` would be identified as a field or property within `GClassX`.

3.  **Tracing Usage: Who *uses* `AfkMonitor`?**
    This is the crucial step. In DnSpy (or any decompiler), you can right-click on a class, method, or field and look for "Analyze" or "Find Usages" (or similar functionality). This tells you what other parts of the code interact with `AfkMonitor`.

    *   If `AfkMonitor` is instantiated or referenced by `MenuOperation` (e.g., `MenuOperation` has a field of type `GClassX` or calls methods on `GClassX`), then you've found the next link in the chain.

4.  **Tracing Further: Who *uses* `MenuOperation`?**
    You repeat the process. If `MenuOperation` (let's say `GClassY`) is found to be used or instantiated by `TarkovApplication` (e.g., `TarkovApplication` has a field of type `GClassY` or calls methods on `GClassY`), then you've found another link.

5.  **Reaching a Known/Root Class (`TarkovApplication`)**
    Eventually, this tracing process leads back to a more fundamental or well-known class, like `TarkovApplication`. `TarkovApplication` is often a central entry point or a top-level manager in Unity games, making it a logical root for many game-related functionalities.

**Why it's not obvious in DnSpy at first glance:**

*   **Obfuscation**: As we discussed, the names are obfuscated. You're not always looking for `TarkovApplication.MenuOperation.AfkMonitor.Delay`. You're looking for `GClassA.GClassB.GClassC.Delay`.
*   **Indirect References**: The connection might not be a direct field. It could be passed as a method argument, returned from a function, or stored in a collection. The "Find Usages" feature of a decompiler is essential for uncovering these indirect links.
*   **Dynamic Instantiation**: Classes might be instantiated dynamically, making it harder to see direct `new` calls. However, the `_dnlibHelper.FindClassWithEntityName` method is designed to handle this by looking for specific methods or properties that uniquely identify a class, regardless of how it's instantiated.

```c#
public void StopAfkMonitor()
{
this.gclass2134_0.Stop();
}
//The class after this keyword is actually a field of the type it's named.

// GClass2134

// Token: 0x0600AEBD RID: 44733 RVA: 0x004E423C File Offset: 0x004E243C

public void Start()
{
    CancellationTokenSource cancellationTokenSource = this.cancellationTokenSource_0;
    if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
    {
        Debug.LogError("AFKMonitor is already running.");
        return;
    }
    this.float_0 = Singleton<GClass1541>.Instance.AFKTimeoutSeconds;
    if (!this.float_0.Positive())
    {
        Debug.LogError("WARNING! AFK timeout is not set.");
        return;
    }
    this.method_0().HandleExceptions();
}
```
Your observation that `this.gclass2134_0` is a field of type `GClass2134` within the class containing `StopAfkMonitor` (which we identified as `GClass2141` / `MenuOperationClass`) is spot on.
Here's how this piece of information fits into the dumper's logic, specifically the line we saw earlier:

```c#
// ... existing code ...
{
	entity = "AfkMonitor";

	var fMethod =_dnlibHelper.FindMethodByName(MenuOperationClass,"StopAfkMonitor");
	var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);
	AfkMonitorClassName = fField.GetTypeName();
	// ... existing code ...
}
// ... existing code ...
```

1.  **`fMethod = _dnlibHelper.FindMethodByName(MenuOperationClass, "StopAfkMonitor");`**
    This line successfully finds the `StopAfkMonitor` method within `MenuOperationClass` (which is `GClass2141`).

2.  **`fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);`**
    This is the critical part. When the dumper calls `GetNthFieldReferencedByMethod` on `StopAfkMonitor`, it's looking at the IL instructions of that method. The instruction `this.gclass2134_0.Stop();` directly references the field `gclass2134_0`. Since this is the *first* field referenced in the method (assuming no other fields are referenced before it), `GetNthFieldReferencedByMethod` (with `N` defaulting to 1) would correctly identify `gclass2134_0`.

3.  **`AfkMonitorClassName = fField.GetTypeName();`**
    Once `fField` is identified as `gclass2134_0`, `GetTypeName()` would return its type, which is `GClass2134`. This is how the dumper programmatically discovers that `GClass2134` is the actual `AfkMonitor` class, even though its name is obfuscated.

This confirms the chain:

*   `TarkovApplication` contains/uses `MenuOperation` (`GClass2141`).
*   `MenuOperation` (`GClass2141`) contains/uses `AfkMonitor` (`GClass2134`), and this usage is identifiable by `StopAfkMonitor` referencing a field of type `GClass2134`.
*   `AfkMonitor` (`GClass2134`) contains the `Delay` (likely `float_0` in your provided `Start` method) which is then extracted.
