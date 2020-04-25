# Silverback + Opentracing

[![Build Status](https://alefcarlos.visualstudio.com/PlusUltra/_apis/build/status/alefcarlos.Silverback.Integration.OpenTracing?branchName=master)](https://alefcarlos.visualstudio.com/PlusUltra/_build/latest?definitionId=24&branchName=master)

[![Nuget](https://img.shields.io/nuget/v/PlusUltra.Silverback.Integration.OpenTracing)](https://www.nuget.org/packages/PlusUltra.Silverback.Integration.OpenTracing/)

Propagates trace information through pub/sub pattern

> Inspired by [this project](https://github.com/yesmarket/MassTransit.OpenTracing)


## How to use

> Ensure that `GlobalTracer` is initialized

Just add `UseOpenTracing()`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSilverback()
        .UseOpenTracing();;
}
```

This will add two [behaviors](https://beagle1984.github.io/silverback/docs/quickstart/behaviors) `ConsumerTracingBehavior` and `ProducerTracingBehavior`.

## Examples

![jaeger](resources/in-memory-jaeger0example.png "JAeger Example")

See `examples/OpenTracing.Example` for full setup


Enjoy ;)
