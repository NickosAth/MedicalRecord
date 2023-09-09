namespace MedicalRecord.Models
{
    public class MedicalHistoryEntry
    {
        public DateTime VisitDateTime { get; set; }
        public string Vaccines { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
    }

}
