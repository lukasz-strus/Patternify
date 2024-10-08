# Patternify.Singleton

This source generator simplifies the implementation of the **Singleton** design pattern in C#. It ensures that a class has only one instance and provides a global point of access to it, while also handling thread safety and lazy initialization.

## Features

- Automatically generates a thread-safe Singleton implementation for your classes.
- Simple and easy-to-use with a single attribute.
- Supports lazy initialization to optimize performance.

## How to Use

### Step 1: Apply the `[Singleton]` Attribute

To transform a class into a Singleton, apply the `[Singleton]` attribute to it. The class must be **partial** to allow the generator to inject code into it.

Example:

```csharp
using Patternify.Singleton;

namespace Samples.Singleton;

[Singleton]
public partial class Configuration
{
    public string SomeProperty { get; set; }
}
```

### Step 2: Let the Generator Do the Work

The generator will automatically create the necessary code to implement the Singleton pattern for the `Configuration` class.

Generated code:

```csharp
// <auto-generated/>
using Patternify.Singleton;

namespace Samples.Singleton
{
    public partial class Configuration
    {
        private static Configuration _instance = null;
        private static object obj = new object();

        private Configuration() { }

        public static Configuration GetInstance()
        {
            lock(obj)
            {
                if (_instance == null)
                {
                    _instance = new Configuration();
                }
            }

            return _instance;
        }
    }
}
```
### Step 3: Access the Singleton

Now you can access the singleton instance using the `GetInstance()` method:

Generated code:

```csharp
var config = Configuration.GetInstance();
config.SomeProperty = "SomeValue";
```

### Installation

To use this source generator in your project, add the NuGet package to your project:

```bash
dotnet add package Patternify.Singleton
```

### Contributing
Contributions are welcome! If you encounter any issues or have ideas for improvements, feel free to open an issue or submit a pull request.

### License

This project is licensed under the MIT License.
