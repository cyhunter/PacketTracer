using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
using System;

internal class Intersections
{
    public Vector256<float> Distances {get; set;}
    public Vector256<int> ThingIndex  {get; set;} 
    // The RayPacket is too big to pass through the function, 
    // so we keep it outside the function and use it carefully

    public static readonly float NullValue = float.MaxValue;

    public Intersections(Vector256<float> dis, Vector256<int> things)
    {
        Distances = dis;
        ThingIndex = things;
    }

    public static Intersections Null = new Intersections(Avx.Set1<float>(Intersections.NullValue), Avx.Set1<int>(-1));

    public bool AllNullIntersections()
    {
        var cmp = Avx.Compare(Distances, Avx.Set1<float>(Intersections.NullValue), FloatComparisonMode.EqualOrderedNonSignaling);
        var zero = Avx.SetZero<int>();
        var mask = Avx2.CompareEqual(zero, zero); 
        return Avx.TestC(cmp, Avx.StaticCast<int, float>(mask));
    }

    public unsafe int[] WithThings()
    {
        var result = new int[VectorPacket.PacketSize];
        fixed (int* ptr = result){
            Avx.Store(ptr, ThingIndex);
        }
        return result;
    }
}