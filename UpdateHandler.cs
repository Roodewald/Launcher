namespace OwlStudio
{
    public delegate void ProgressBarUpdateHandler(object sender, UpdateData e);
    public class UpdateData : EventArgs
    {
        public long ProcessedData { get; set; }
        public long TotalData { get; set; }
        public uint Mode { get; set; }
    }
}