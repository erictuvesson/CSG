namespace CSG.GitHub
{
    using CSG.Content.STL;
    using CSG.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using Xunit;
    using Xunit.Abstractions;

    public class Issue21 : Test
    {
        const string hash1 = "MTAuMDQxNjY3IC0wLjI0MjAyODk4IDAKNDcuOTE2NjY4IDAuNDE2NjY2NjYgMAo0Ny45MTY2NjggLTAuNDE2NjY2NjYgMAoxMC4wNDE2NjcgLTAuNDE2NjY2NjYgMAoxMC4wNDE2NjcgLTAuMjQyMDI4OTggMAo0Ny45MTY2NjggLTAuNDE2NjY2NjYgMAoxMC4wNDE2NjcgMC40MTY2NjY2NiAwCjQ3LjkxNjY2OCAwLjQxNjY2NjY2IDAKMTAuMDQxNjY3IC0wLjI0MjAyODk4IDAKMS44NzUgLTAuNDE2NjY2NyAwCjAgLTAuNDE2NjY2NjYgMAoxLjg3NSAtMC4zODQwNTc5NyAwCjEuODc1IC0wLjM4NDA1Nzk3IDAKMCAtMC40MTY2NjY2NiAwCjAgMC40MTY2NjY2NiAwCjEuODc1IDAuNDE2NjY2NyAwCjEuODc1IC0wLjM4NDA1Nzk3IDAKMCAwLjQxNjY2NjY2IDAKNDcuOTE2NjY4IC0wLjQxNjY2NjY2IDAKNDcuOTE2NjY4IC0wLjQxNjY2NjY2IDExCjEwLjA0MTY2NyAtMC40MTY2NjY2NiAyLjMwNTIxNzUKMTAuMDQxNjY3IC0wLjQxNjY2NjY2IDAKNDcuOTE2NjY4IC0wLjQxNjY2NjY2IDAKMTAuMDQxNjY3IC0wLjQxNjY2NjY2IDIuMzA1MjE3NQoxMC4wNDE2NjcgLTAuNDE2NjY2NjYgMTEKMTAuMDQxNjY3IC0wLjQxNjY2NjY2IDIuMzA1MjE3NQo0Ny45MTY2NjggLTAuNDE2NjY2NjYgMTEKMTAuMDQxNjY3IC0wLjQxNjY2NjY2IDExCjAgLTAuNDE2NjY2NjYgMTEKMCAtMC40MTY2NjY2NiA3LjkxNjY2NwoxMC4wNDE2NjcgLTAuNDE2NjY2NjYgNy45MTY2NjcKMTAuMDQxNjY3IC0wLjQxNjY2NjY2IDExCjAgLTAuNDE2NjY2NjYgNy45MTY2NjcKMCAtMC40MTY2NjY2NiA3LjkxNjY2NwowIC0wLjQxNjY2NjY2IDExCjAgLTAuMTgzMDgwODIgNy45MTY2NjcKMCAwLjQxNjY2NjY2IDcuOTE2NjY3CjAgLTAuMTgzMDgwODIgNy45MTY2NjcKMCAtMC40MTY2NjY2NiAxMQowIDAuNDE2NjY2NjYgMTEKMCAwLjQxNjY2NjY2IDcuOTE2NjY3CjAgLTAuNDE2NjY2NjYgMTEKMCAtMC40MTY2NjY2NiA3LjkxNjY2NwowIC0wLjE4MzA4MDgyIDcuOTE2NjY3CjAgMC40MTY2NjY2NiAwCjAgLTAuNDE2NjY2NjYgMAowIC0wLjQxNjY2NjY2IDcuOTE2NjY3CjAgMC40MTY2NjY2NiAwCjAgMC40MTY2NjY2NiA3LjkxNjY2NwowIDAuNDE2NjY2NjYgMAowIC0wLjE4MzA4MDgyIDcuOTE2NjY3CjEwLjA0MTY2NyAwLjQxNjY2NjY2IDAKMTAuMDQxNjY3IDAuNDE2NjY2NjYgOC42OTQ3ODIKNDcuOTE2NjY4IDAuNDE2NjY2NjYgMAo0Ny45MTY2NjggMC40MTY2NjY2NiAxMQo0Ny45MTY2NjggMC40MTY2NjY2NiAwCjEwLjA0MTY2NyAwLjQxNjY2NjY2IDguNjk0NzgyCjEwLjA0MTY2NyAwLjQxNjY2NjY2IDExCjQ3LjkxNjY2OCAwLjQxNjY2NjY2IDExCjEwLjA0MTY2NyAwLjQxNjY2NjY2IDguNjk0NzgyCjAgMC40MTY2NjY2NiA3LjkxNjY2NwowIDAuNDE2NjY2NjYgMTEKMTAuMDQxNjY3IDAuNDE2NjY2NjYgOC42OTQ3ODIKMTAuMDQxNjY3IDAuNDE2NjY2NjYgNy45MTY2NjcKMCAwLjQxNjY2NjY2IDcuOTE2NjY3CjEwLjA0MTY2NyAwLjQxNjY2NjY2IDguNjk0NzgyCjEwLjA0MTY2NyAwLjQxNjY2NjY2IDExCjEwLjA0MTY2NyAwLjQxNjY2NjY2IDguNjk0NzgyCjAgMC40MTY2NjY2NiAxMQo0Ny45MTY2NjggMC40MTY2NjY2NiAwCjQ3LjkxNjY2OCAwLjQxNjY2NjY2IDExCjQ3LjkxNjY2OCAtMC40MTY2NjY2NiAwCjQ3LjkxNjY2OCAtMC40MTY2NjY2NiAxMQo0Ny45MTY2NjggLTAuNDE2NjY2NjYgMAo0Ny45MTY2NjggMC40MTY2NjY2NiAxMQoxMC4wNDE2NjcgMC40MTY2NjY2NiAxMQoxMC4wNDE2NjcgLTAuMjQyMDI4OTggMTEKNDcuOTE2NjY4IDAuNDE2NjY2NjYgMTEKNDcuOTE2NjY4IC0wLjQxNjY2NjY2IDExCjQ3LjkxNjY2OCAwLjQxNjY2NjY2IDExCjEwLjA0MTY2NyAtMC4yNDIwMjg5OCAxMQoxMC4wNDE2NjcgLTAuNDE2NjY2NjYgMTEKNDcuOTE2NjY4IC0wLjQxNjY2NjY2IDExCjEwLjA0MTY2NyAtMC4yNDIwMjg5OCAxMQowIDAuNDE2NjY2NjYgMTEKMCAtMC40MTY2NjY2NiAxMQoxMC4wNDE2NjcgLTAuMjQyMDI4OTggMTEKMTAuMDQxNjY3IDAuNDE2NjY2NjYgMTEKMCAwLjQxNjY2NjY2IDExCjEwLjA0MTY2NyAtMC4yNDIwMjg5OCAxMQoxMC4wNDE2NjcgLTAuNDE2NjY2NjYgMTEKMTAuMDQxNjY3IC0wLjI0MjAyODk4IDExCjAgLTAuNDE2NjY2NjYgMTEKMTAuMDQxNjY4IC0wLjQxNjY2NjYzIDMuNzI2MzMzNgoxMC4wNDE2NjggLTAuNDE2NjY2NSA0LjU4MzMzMzUKMTAuMDQxNjY3IDAuNDE2NjY2NjYgMy45MTY2NjY3CjEwLjA0MTY2OCAtMC40MTY2NjY2NiAxLjgxMjc1MDYKMTAuMDQxNjY4IC0wLjQxNjY2NjYzIDMuNzI2MzMzNgoxMC4wNDE2NjcgMC40MTY2NjY2NiAzLjkxNjY2NjcKMTAuMDQxNjY4IC0wLjQxNjY2NjYzIDEuNDIzNDUzRS0wNwoxMC4wNDE2NjggLTAuNDE2NjY2NjYgMS44MTI3NTA2CjEwLjA0MTY2NyAwLjQxNjY2NjY2IDMuOTE2NjY2NwoxMC4wNDE2NjYgMC40MTY2NjY2NiAxLjIxNjQwNTFFLTA3CjEwLjA0MTY2OCAtMC40MTY2NjY2MyAxLjQyMzQ1M0UtMDcKMTAuMDQxNjY3IDAuNDE2NjY2NjYgMy45MTY2NjY3CjEwLjA0MTY2NiAtMC40MTY2NjY2NiA0LjU4MzMzMwoxMC4wNDE2NjcgLTAuNDE2NjY2NjYgNy45MTY2NjcKMTAuMDQxNjY3IDAuNDE2NjY2NSA3LjkxNjY2NwoxMC4wNDE2NjcgMC40MTY2NjY2MyAzLjkxNjY2NjUKMTAuMDQxNjY2IC0wLjQxNjY2NjY2IDQuNTgzMzMzCjEwLjA0MTY2NyAwLjQxNjY2NjUgNy45MTY2NjcKMTAuMDQxNjY4IC0wLjQxNjY2NjY2IDEuODEyNzUwNgoxMC4wNDE2NjggLTAuNDE2NjY2NyAzLjYwMjM5OTMKMTAuMDQxNjY4IC0wLjQxNjY2NjYzIDMuNzI2MzMzNgo1Ljk1ODMzNCAwLjQxNjY2NjUgNy45MTY2NjcKMTAuMDQxNjY3IDAuNDE2NjY2NiA3LjkxNjY2NwoxMC4wNDE2NjcgLTAuNDE2NjY2NjMgNy45MTY2NjcKNS4yNzc3NzggLTAuNDE2NjY2NjYgNy45MTY2NjcKNS45NTgzMzQgMC40MTY2NjY1IDcuOTE2NjY3CjEwLjA0MTY2NyAtMC40MTY2NjY2MyA3LjkxNjY2Nwo1LjI3Nzc3OCAtMC40MTY2NjY2MyA3LjkxNjY2NwoxLjg3NSAtMC40MTY2NjY2NiA3LjkxNjY2NwoxLjg3NSAwLjQxNjY2NjUgNy45MTY2NjcKNS45NTgzMzM1IDAuNDE2NjY2NiA3LjkxNjY2Nwo1LjI3Nzc3OCAtMC40MTY2NjY2MyA3LjkxNjY2NwoxLjg3NSAwLjQxNjY2NjUgNy45MTY2NjcKMS44NzUgMC40MTY2NjY2NiA2LjY5MjA0OAoxLjg3NTAwMDEgMC40MTY2NjY2IDcuOTE2NjY3CjEuODc1IC0wLjQxNjY2NjYzIDcuOTE2NjY3CjEuODc1IDAuNDE2NjY2NjMgMy45MTY2NjcKMS44NzUgMC40MTY2NjY2NiA2LjY5MjA0OAoxLjg3NSAtMC40MTY2NjY2MyA3LjkxNjY2NwoxLjg3NSAtMC40MTY2NjY3IDMuMjUwMDAwNQoxLjg3NSAwLjQxNjY2NjYzIDMuOTE2NjY3CjEuODc1IC0wLjQxNjY2NjYzIDcuOTE2NjY3CjEuODc1IC0wLjQxNjY2NjY2IDUuNTE2NTYxNQoxLjg3NSAtMC40MTY2NjY3IDMuMjUwMDAwNQoxLjg3NSAtMC40MTY2NjY2MyA3LjkxNjY2NwoxLjg3NSAwLjQxNjY2NjUgMS4yMTY0MDU0RS0wNwoxLjg3NTAwMDIgMC40MTY2NjY2IDMuOTE2NjY3MgoxLjg3NTAwMDIgLTAuNDE2NjY2NjMgMy4yNTAwMDA1CjEuODc1MDAwMSAtMC40MTY2NjY2NiAxLjAwOTM1NzY1RS0wNwoxLjg3NSAwLjQxNjY2NjUgMS4yMTY0MDU0RS0wNwoxLjg3NTAwMDIgLTAuNDE2NjY2NjMgMy4yNTAwMDA1CjEuODc1IDAuNDE2NjY2NyAwCjAgMC40MTY2NjY2NiAwCjAgMC40MTY2NjY2NiA3LjkxNjY2NwoxLjg3NSAwLjQxNjY2NjcgNy45MTY2NjcKMS44NzUgMC40MTY2NjY3IDAKMCAwLjQxNjY2NjY2IDcuOTE2NjY3CjEwLjA0MTY2NiAwLjQxNjY2NjY2IDEuMjE2NDA1MUUtMDcKMTAuMDQxNjY3IDAuNDE2NjY2NjYgMy45MTY2NjY3CjEwLjA0MTY2OCAwLjQxNjY2NjcgMy45MTY2NjY3CjEuODc1IC0wLjQxNjY2NjcgMAoxLjg3NSAtMC40MTY2NjY3IDAuNDMwNDM0NzYKMCAtMC40MTY2NjY2NiAwCjAgLTAuNDE2NjY2NjYgNy45MTY2NjcKMCAtMC40MTY2NjY2NiAwCjEuODc1IC0wLjQxNjY2NjcgMC40MzA0MzQ3NgoxLjg3NSAtMC40MTY2NjY3IDcuOTE2NjY3CjAgLTAuNDE2NjY2NjYgNy45MTY2NjcKMS44NzUgLTAuNDE2NjY2NyAwLjQzMDQzNDc2CjEuODc1IC0wLjQxNjY2NjY2IDUuNTE2NTYxNQoxLjg3NSAtMC40MTY2NjY3IDMuMjUwMDAwNQoxLjg3NSAtMC40MTY2NjY3IDMuMjUwMDAwNQoxMC4wNDE2NjggLTAuNDE2NjY2NjYgMS44MTI3NTA2CjEwLjA0MTY2OCAtMC40MTY2NjY3IDMuNTI0NjM3NQoxMC4wNDE2NjggLTAuNDE2NjY2NyAzLjYwMjM5OTMKMTAuMDQxNjY2IC0wLjQxNjY2NjY2IDQuNTgzMzMzCjEwLjA0MTY2NyAtMC40MTY2NjY3IDQuNTgzMzMzNQoxMC4wNDE2NjcgLTAuNDE2NjY2NjYgNy45MTY2NjcKMS44NzUgLTAuNDE2NjY2NjYgNS41MTY1NjE1CjEuODc1IC0wLjQxNjY2NjcgMy4yNTAwMDA1CjEuODc1IC0wLjQxNjY2NjcgMy4yNTAwMDA1Cg==";
        const string hash2 = "MjUuODc1IC00LjU4MzMzMzUgLTAuMDgzMzMzMjU0CjM0LjA0MTY2OCAtNC41ODMzMzM1IDcuOTE2NjY3CjI1Ljg3NSAtNC41ODMzMzM1IDcuOTE2NjY3CjM0LjA0MTY2OCAtNC41ODMzMzM1IDcuOTE2NjY3CjI1Ljg3NSAtNC41ODMzMzM1IC0wLjA4MzMzMzI1NAozNC4wNDE2NjggLTQuNTgzMzMzNSAtMC4wODMzMzMyNTQKMzQuMDQxNjY4IC00LjU4MzMzMzUgLTAuMDgzMzMzMjU0CjM0LjA0MTY2OCA1LjQxNjY2NjUgLTAuMDgzMzMzMjU0CjM0LjA0MTY2OCAtNC41ODMzMzM1IDcuOTE2NjY3CjM0LjA0MTY2OCA1LjQxNjY2NjUgNy45MTY2NjcKMzQuMDQxNjY4IC00LjU4MzMzMzUgNy45MTY2NjcKMzQuMDQxNjY4IDUuNDE2NjY2NSAtMC4wODMzMzMyNTQKMzQuMDQxNjY4IC00LjU4MzMzMzUgNy45MTY2NjcKMzQuMDQxNjY4IDUuNDE2NjY2NSA3LjkxNjY2NwoyNS44NzUgLTQuNTgzMzMzNSA3LjkxNjY2NwoyNS44NzUgNS40MTY2NjY1IDcuOTE2NjY3CjI1Ljg3NSAtNC41ODMzMzM1IDcuOTE2NjY3CjM0LjA0MTY2OCA1LjQxNjY2NjUgNy45MTY2NjcKMjUuODc1IC00LjU4MzMzMzUgNy45MTY2NjcKMjUuODc1IDUuNDE2NjY2NSA3LjkxNjY2NwoyNS44NzUgLTQuNTgzMzMzNSAtMC4wODMzMzMyNTQKMjUuODc1IDUuNDE2NjY2NSAtMC4wODMzMzMyNTQKMjUuODc1IC00LjU4MzMzMzUgLTAuMDgzMzMzMjU0CjI1Ljg3NSA1LjQxNjY2NjUgNy45MTY2NjcKMjUuODc1IC00LjU4MzMzMzUgLTAuMDgzMzMzMjU0CjI1Ljg3NSA1LjQxNjY2NjUgLTAuMDgzMzMzMjU0CjM0LjA0MTY2OCAtNC41ODMzMzM1IC0wLjA4MzMzMzI1NAozNC4wNDE2NjggNS40MTY2NjY1IC0wLjA4MzMzMzI1NAozNC4wNDE2NjggLTQuNTgzMzMzNSAtMC4wODMzMzMyNTQKMjUuODc1IDUuNDE2NjY2NSAtMC4wODMzMzMyNTQKMzQuMDQxNjY4IDUuNDE2NjY2NSAtMC4wODMzMzMyNTQKMjUuODc1IDUuNDE2NjY2NSAtMC4wODMzMzMyNTQKMzQuMDQxNjY4IDUuNDE2NjY2NSA3LjkxNjY2NwoyNS44NzUgNS40MTY2NjY1IDcuOTE2NjY3CjM0LjA0MTY2OCA1LjQxNjY2NjUgNy45MTY2NjcKMjUuODc1IDUuNDE2NjY2NSAtMC4wODMzMzMyNTQK";

        public Issue21(ITestOutputHelper output)
            : base(output)
        { }

        [Fact]
        public void Reproduce()
        {
            // Parse the vertices data
            List<Vector3> points1 = ReadVertices(hash1);
            List<Vector3> points2 = ReadVertices(hash2);

            Assert.NotEmpty(points1);
            Assert.NotEmpty(points2);

            // Assume that 3 vertices make a triangles. (bad solution)
            List<Vertex> vertices1 = points1.Select(x => new Vertex(x, new Vector4(0, 0, 1, 1))).ToList();
            List<Vertex> vertices2 = points2.Select(x => new Vertex(x, new Vector4(1, 0, 0, 1))).ToList();

            // Create polygons
            Polygon polygon1 = new Polygon(vertices1);
            Polygon polygon2 = new Polygon(vertices2);

            // Create shapes
            Shape shape1 = new GeneratedShape(new[] { polygon1 });
            Shape shape2 = new GeneratedShape(new[] { polygon2 });

            // Execute operation
            Assert.Throws<BSPNodeMaxDepthException>(() =>
            {
                shape1.Subtract(shape2);
            });
        }

        [Fact]
        public void Solution()
        {
            // Parse the vertices data
            List<Vector3> points1 = ReadVertices(hash1);
            List<Vector3> points2 = ReadVertices(hash2);

            Assert.NotEmpty(points1);
            Assert.NotEmpty(points2);

            // Assume that 3 vertices make a triangles. (bad solution)
            List<Vertex> vertices1 = points1.Select(x => new Vertex(x, new Vector4(0, 0, 1, 1))).ToList();
            List<Vertex> vertices2 = points2.Select(x => new Vertex(x, new Vector4(1, 0, 0, 1))).ToList();

            // Create polygons
            List<Polygon> polygons1 = vertices1.Chunk(3)
                .Select(x => new Polygon(x))
                .ToList();

            List<Polygon> polygons2 = vertices2.Chunk(3)
                .Select(x => new Polygon(x))
                .ToList();

            // Create shapes
            Shape shape1 = new GeneratedShape(polygons1);
            Shape shape2 = new GeneratedShape(polygons2);

            // Execute operation
            Shape subtractedShape = shape1.Subtract(shape2);

            Assert.NotEmpty(subtractedShape.Vertices);

            // Write to STL for preview
            var writer = new StlBinaryContentWriter();
            System.IO.File.WriteAllBytes("./issue-21-shape1.stl", writer.Write(shape1));
            System.IO.File.WriteAllBytes("./issue-21-shape2.stl", writer.Write(shape2));
            System.IO.File.WriteAllBytes("./issue-21-operation-subtract.stl", writer.Write(subtractedShape));
        }

        private List<Vector3> ReadVertices(string hash)
        {
            List<Vector3> vertices = new List<Vector3>();
            hash = Encoding.UTF8.GetString(Convert.FromBase64String(hash));
            List<string> verts = hash.Split('\n').ToList();
            foreach (string s in verts.Where(x => !string.IsNullOrEmpty(x)))
            {
                string[] pts = s.Split(' ');
                float.TryParse(pts[0], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var x);
                float.TryParse(pts[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var y);
                float.TryParse(pts[2], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var z);
                vertices.Add(new Vector3(x, y, z));
            }

            // Make the list unique
            return vertices.ToList();
        }
    }
}
