namespace WkndRay.Pdfs
{
  public class HitablePdf : IPdf
  {
    public HitablePdf(IHitable hitable, PosVector origin)
    {
      Hitable = hitable;
      Origin = origin;
    }

    public IHitable Hitable { get; }
    public PosVector Origin { get; }

    public double GetValue(PosVector direction)
    {
      return Hitable.GetPdfValue(Origin, direction);
    }

    public PosVector Generate()
    {
      return Hitable.Random(Origin);
    }
  }
}
