namespace WebApplication1.Entities;

public class FileUpdateReadModel
{
    public Guid Id { get; set; }
    public int Code { get; set; }
    public string MimeType { get; set; }
    public long Size { get; set; }
    public string OriginalName { get; set; }
    public string Name { get; set; }
    public byte[] Data { get; set; }

}