// using BenchmarkDotNet.Attributes;
// using BenchmarkDotNet.Order;
// using passwordManagent2.Services; // Usa el namespace real de tu servicio

// [MemoryDiagnoser]
// [Orderer(SummaryOrderPolicy.FastestToSlowest)]
// [RankColumn]
// public class PasswordServiceBenchmark
// {
//     private readonly PasswordService _service = new();

//     [Benchmark]
//     public void EncryptPassword()
//     {
//         _service.Encrypt("MiClaveSuperSegura123!");
//     }
// }

