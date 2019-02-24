# Constructive solid geometry (CSG)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/28566da230d24b50807cb66be775aa22)](https://app.codacy.com/app/eric-tuvesson/CSG?utm_source=github.com&utm_medium=referral&utm_content=erictuvesson/CSG&utm_campaign=Badge_Grade_Dashboard)
[![NuGet Badge](https://buildstats.info/nuget/CSGeometry)](https://www.nuget.org/packages/CSGeometry/)

Create geometry objects from boolean operations.

Support serialization with [Mallos.Serialization](https://github.com/erictuvesson/Mallos.Serialization)

## Sample

```csharp
var shape1 = new Cube(position: new Vector3(0, 0, 0), size: new Vector3(0.50f, 0.50f, 0.50f));
var shape2 = new Cube(position: new Vector3(1, 1, 0), size: new Vector3(0.50f, 0.50f, 0.50f));
var result = shape1.Intersect(shape2);

// result.Vertices => Vertex[];
// result.Indices => ushort[];
// then you have the vertices and indices in result.
```

## Contributing
Contribution directions go here.

## License
The project is available as open source under the terms of the [MIT License](http://opensource.org/licenses/MIT).
