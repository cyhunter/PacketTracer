using System.Runtime.CompilerServices.Intrinsics.Intel;
using System.Runtime.CompilerServices.Intrinsics;

internal class PlanePacket: ObjectPacket
{
    public VectorPacket Norms {get; private set;}
    public Vector256<float> Offsets {get; private set;}

    public PlanePacket(Plane plane): base(plane.Surface)
    {
        Norms = new VectorPacket(AVX.Set1(plane.Norm.X), AVX.Set1(plane.Norm.Y), AVX.Set1(plane.Norm.Z));
        Offsets = AVX.Set1(plane.Offset);
    }

    public override Intersections Intersect(RayPacket rayPacket)
    {
        var denom = VectorPacket.DotProduct(Norms, rayPacket.Dirs)
        var dist = AVX.Divide(VectorPacket.DotProduct(Norms, rayPacket.Starts), AVX.Subtract(AVX.SetZero<float>(), denom));
        var gtMask = AVX.CompareVector256Float(denom, AVX.SetZero<float>, CompareGreaterThanOrderedNonSignaling);
        var reslut = AVX.Or(AVX.And(gtMask, AVX.Set1(Intersections.Null)), AVX.AndNot(gtMask, dist));
        return new Intersections(reslut, this);
    }
}