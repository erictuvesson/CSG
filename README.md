# Constructive solid geometry (CSG)
Create geometry objects from boolean operations.

This project was inspired by [pb_CSG](https://github.com/karl-/pb_CSG), which was designed for Unity. 

## Sample

```csharp
var shape1 = new Cube(position: new Vector3(0, 0, 0), size: new Vector3(0.50f, 0.50f, 0.50f));
var shape2 = new Cube(position: new Vector3(1, 1, 0), size: new Vector3(0.50f, 0.50f, 0.50f));
var result = shape1.Intersect(shape2);

// result.Cache.Vertices => Vertex[];
// result.Cache.Indices => ushort[];
// then you have the vertices and indices in result.
```

## Contributing
Contribution directions go here.

## License
The project is available as open source under the terms of the [MIT License](http://opensource.org/licenses/MIT).
