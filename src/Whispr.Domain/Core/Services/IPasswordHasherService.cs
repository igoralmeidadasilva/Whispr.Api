namespace Whispr.Domain.Core.Services;

public interface IPasswordHasherService
{
    const int DEFAULT_WORK_FACTOR = 12;
    string Hash(string password, int workFactor = DEFAULT_WORK_FACTOR);
    bool Verify(string password, string hash);
}