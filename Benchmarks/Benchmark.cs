using BenchmarkDotNet.Attributes;
// using BenchmarkDotNet.Running;
using passwordManagent2.services;

namespace passwordManagent2.Benchmark
{
    [MemoryDiagnoser]
    [RankColumn]
    // [SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net60)] // Especifica la versión de .NET
    public class BenchmarkClass
    {
        private EncryptionPassword? _encryptionService;
        private string _encryptedPassword = string.Empty;
        private const string TestPassword = "200211";
        private const string TestKey = "200211";

        [GlobalSetup]
        public void Setup()
        {
            _encryptionService = new EncryptionPassword();
            // Pre-calcula el password encriptado para el benchmark de decrypt
            _encryptedPassword = _encryptionService.Encrypt(TestPassword, TestKey);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _encryptionService?.Dispose();
        }

        [Benchmark(Baseline = true)]
        public string EncryptPassword()
        {
            return _encryptionService!.Encrypt(TestPassword, TestKey);
        }

        [Benchmark]
        public string DecryptPassword()
        {
            return _encryptionService!.Decrypt(_encryptedPassword, TestKey);
        }

        // Benchmark adicional para medir el ciclo completo
        [Benchmark]
        public string EncryptDecryptCycle()
        {
            string encrypted = _encryptionService!.Encrypt(TestPassword, TestKey);
            return _encryptionService.Decrypt(encrypted, TestKey);
        }
    }
}