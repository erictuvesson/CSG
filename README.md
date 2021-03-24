# Constructive solid geometry (CSG)
[![NuGet Badge](https://buildstats.info/nuget/CSGeometry)](https://www.nuget.org/packages/CSGeometry/) [![Codacy Badge](https://api.codacy.com/project/badge/Grade/f3ca20ee0dab4a5287dfca8fc41326d8)](https://www.codacy.com/app/eric-tuvesson/CSG?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=erictuvesson/CSG&amp;utm_campaign=Badge_Grade)

Create geometry objects from boolean operations.

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
Contributions are always welcome.

## License
The project is available as open source under the terms of the [MIT License](http://opensource.org/licenses/MIT).
