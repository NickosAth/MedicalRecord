namespace MedicalRecord.Models
{
    public class MedicalHistoryModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime VisitDate { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
    }

}
