using System;

namespace Sr3D.SrMath
{
    public class Matrix4x4 : ICloneable
    {
        public static Matrix4x4 Identity => new Matrix4x4()
        {
            M11 = 1,
            M22 = 1,
            M33 = 1,
            M44 = 1
        };

        public float M11;
        public float M44;
        public float M43;
        public float M42;
        public float M41;
        public float M34;
        public float M33;
        public float M31;
        public float M32;
        public float M23;
        public float M22;
        public float M21;
        public float M14;
        public float M13;
        public float M12;
        public float M24;

        public static Vector4 operator *(Matrix4x4 m1, Vector4 v1)
        {
            Vector4 result = new Vector4();

            result.X = m1.M11 * v1.X + m1.M12 * v1.Y + m1.M13 * v1.Z + m1.M14 * v1.W;
            result.Y = m1.M21 * v1.X + m1.M22 * v1.Y + m1.M23 * v1.Z + m1.M24 * v1.W;
            result.Z = m1.M31 * v1.X + m1.M32 * v1.Y + m1.M33 * v1.Z + m1.M34 * v1.W;
            result.W = m1.M41 * v1.X + m1.M42 * v1.Y + m1.M43 * v1.Z + m1.M44 * v1.W;

            return result;
        }

        public static Matrix4x4 operator *(Matrix4x4 m1, Matrix4x4 m2)
        {
            Matrix4x4 result = new Matrix4x4();

            result.M11 = m1.M11 * m2.M11 + m1.M12 * m2.M21 + m1.M13 * m2.M31 + m1.M14 * m2.M41;
            result.M12 = m1.M11 * m2.M12 + m1.M12 * m2.M22 + m1.M13 * m2.M32 + m1.M14 * m2.M42;
            result.M13 = m1.M11 * m2.M13 + m1.M12 * m2.M23 + m1.M13 * m2.M33 + m1.M14 * m2.M43;
            result.M14 = m1.M11 * m2.M14 + m1.M12 * m2.M24 + m1.M13 * m2.M34 + m1.M14 * m2.M44;
                                                                                      
            result.M21 = m1.M21 * m2.M11 + m1.M22 * m2.M21 + m1.M23 * m2.M31 + m1.M24 * m2.M41;
            result.M22 = m1.M21 * m2.M12 + m1.M22 * m2.M22 + m1.M23 * m2.M32 + m1.M24 * m2.M42;
            result.M23 = m1.M21 * m2.M13 + m1.M22 * m2.M23 + m1.M23 * m2.M33 + m1.M24 * m2.M43;
            result.M24 = m1.M21 * m2.M14 + m1.M22 * m2.M24 + m1.M23 * m2.M34 + m1.M24 * m2.M44;
                                                                                      
            result.M31 = m1.M31 * m2.M11 + m1.M32 * m2.M21 + m1.M33 * m2.M31 + m1.M34 * m2.M41;
            result.M32 = m1.M31 * m2.M12 + m1.M32 * m2.M22 + m1.M33 * m2.M32 + m1.M34 * m2.M42;
            result.M33 = m1.M31 * m2.M13 + m1.M32 * m2.M23 + m1.M33 * m2.M33 + m1.M34 * m2.M43;
            result.M34 = m1.M31 * m2.M14 + m1.M32 * m2.M24 + m1.M33 * m2.M34 + m1.M34 * m2.M44;
                                                                                      
            result.M41 = m1.M41 * m2.M11 + m1.M42 * m2.M21 + m1.M43 * m2.M31 + m1.M44 * m2.M41;
            result.M42 = m1.M41 * m2.M12 + m1.M42 * m2.M22 + m1.M43 * m2.M32 + m1.M44 * m2.M42;
            result.M43 = m1.M41 * m2.M13 + m1.M42 * m2.M23 + m1.M43 * m2.M33 + m1.M44 * m2.M43;
            result.M44 = m1.M41 * m2.M14 + m1.M42 * m2.M24 + m1.M43 * m2.M34 + m1.M44 * m2.M44;

            return result;
        }

        public static Matrix4x4 CreateRotationZ(float radians)
        {
            var sin = (float)Math.Sin(radians);
            var cos = (float)Math.Cos(radians);

            // | cos(r)   sin(r)   0   0 |
            // | -sin(r)  cos(r)   0   0 |
            // | 0        0        1   0 |
            // | 0        0        0   1 |

            Matrix4x4 rotation = Identity;
            rotation.M11 = cos;
            rotation.M12 = sin;
            rotation.M21 = -sin;
            rotation.M22 = cos;

            return rotation;
        }

        public static Matrix4x4 CreateRotationY(float radians)
        {
            var sin = (float)Math.Sin(radians);
            var cos = (float)Math.Cos(radians);

            // | cos(r)   0   sin(r)   0 |
            // | 0        1   0        0 |
            // | -sin(r)  0   cos(r)   0 |
            // | 0        0        0   1 |

            Matrix4x4 rotation = Identity;
            rotation.M11 = cos;
            rotation.M13 = sin;
            rotation.M31 = -sin;
            rotation.M33 = cos;

            return rotation;
        }

        public static Matrix4x4 CreateRotationX(float radians)
        {
            var sin = (float)Math.Sin(radians);
            var cos = (float)Math.Cos(radians);

            // | 1  0        0       0 |
            // | 0  cos(r)   sin(r)  0 |
            // | 0  -sin(r)  cos(r)  0 |
            // | 0  0        0       1 |

            Matrix4x4 rotation = Identity;
            rotation.M22 = cos;
            rotation.M23 = sin;
            rotation.M32 = -sin;
            rotation.M33 = cos;

            return rotation;
        }

        public static Matrix4x4 CreateTranslation(float x, float y, float z)
        {
            // | 1  0  0  x |
            // | 0  1  0  y |
            // | 0  0  1  z |
            // | 0  0  0  1 |

            Matrix4x4 translation = Identity;
            translation.M14 = x;
            translation.M24 = y;
            translation.M34 = z;

            return translation;
        }

        public object Clone()
        {
            var clone = new Matrix4x4();

            clone.M11 = M11;
            clone.M12 = M12;
            clone.M13 = M13;
            clone.M14 = M14;
            clone.M21 = M21;
            clone.M22 = M22;
            clone.M23 = M23;
            clone.M24 = M24;
            clone.M31 = M31;
            clone.M32 = M32;
            clone.M33 = M33;
            clone.M34 = M34;
            clone.M41 = M41;
            clone.M42 = M42;
            clone.M43 = M43;
            clone.M44 = M44;

            return clone;
        }
    }
}
