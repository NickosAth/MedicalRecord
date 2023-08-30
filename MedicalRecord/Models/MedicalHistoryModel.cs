namespace MedicalRecord.Models
{
    public class MedicalHistoryModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Vaccines { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public DateTime VisitDateTime { get; set; }
    }
}
