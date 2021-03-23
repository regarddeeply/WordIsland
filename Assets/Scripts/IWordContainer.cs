public interface IWordContainer
{
    bool IsFull { get; }
    void AddLetter(string letter);
    void Remove(string letter);
    string RemoveAt(int id);
}
