using System;
using System.Collections.Generic;

namespace JogoArvores
{
    // ESTADO INTRÍNSECO (Flyweight)
    public class EspecieArvore
    {
        public string Nome { get; }
        public string TexturaCasca { get; }
        public string SpriteFolhas { get; }

        public EspecieArvore(string nome, string textura, string sprite)
        {
            Nome = nome;
            TexturaCasca = textura;
            SpriteFolhas = sprite;
        }
    }

    // FLYWEIGHT FACTORY (Garante apenas 1 instância por espécie)
    public class EspecieFactory
    {
        private readonly Dictionary<string, EspecieArvore> _especies = new();

        public EspecieArvore ObterEspecie(string nome)
        {
            if (!_especies.ContainsKey(nome))
            {
                // Simula dados pesados de textura e sprites
                _especies[nome] = new EspecieArvore(nome, $"Textura_Pesada_Casca_{nome}.png", $"Sprite_HD_Folhas_{nome}.png");
            }
            return _especies[nome];
        }

        public int TotalEspeciesCriadas() => _especies.Count;
    }

    // ESTADO EXTRÍNSECO (Características mutáveis de cada árvore) 
    public class ArvoreIndividual
    {
        // Posição e dimensões únicas de cada árvore
        public float X { get; }
        public float Y { get; }
        public float Altura { get; }
        public float Diametro { get; }
        public int NumeroGalhos { get; }
        
        // Referência partilhada para o objeto Flyweight 
        public EspecieArvore Especie { get; }

        public ArvoreIndividual(float x, float y, float altura, float diametro, int galhos, EspecieArvore especie)
        {
            X = x;
            Y = y;
            Altura = altura;
            Diametro = diametro;
            NumeroGalhos = galhos;
            Especie = especie;
        }
    }

    // EXECUÇÃO E CÁLCULO DE MEMÓRIA 
    class Program
    {
        static void Main(string[] args)
        {
            EspecieFactory fabrica = new EspecieFactory();
            List<ArvoreIndividual> terreno = new();
            Random random = new Random();

            int quantidadeArvores = 50000; // Quantidade de árvores no mapa (x mil)
            int quantidadeEspecies = 50;   // Limite de 50 espécies diferentes

            Console.WriteLine($"GERANDO TERRENO COM {quantidadeArvores} ÁRVORES E {quantidadeEspecies} ESPÉCIES");

            // Criar a simulação do florestamento
            for (int i = 0; i < quantidadeArvores; i++)
            {
                // Sorteia uma das 50 espécies
                string nomeEspecie = $"Especie_{random.Next(1, quantidadeEspecies + 1)}";
                EspecieArvore especieCompartilhada = fabrica.ObterEspecie(nomeEspecie);

                // Gera características únicas e mutáveis para esta árvore específica
                float posX = (float)(random.NextDouble() * 1000);
                float posY = (float)(random.NextDouble() * 1000);
                float altura = (float)(2.0 + random.NextDouble() * 15.0); // entre 2m e 17m
                float diametro = (float)(0.2 + random.NextDouble() * 2.0); // entre 20cm e 2.2m
                int galhos = random.Next(5, 60);

                // Cria a árvore utilizando o Flyweight
                terreno.Add(new ArvoreIndividual(posX, posY, altura, diametro, galhos, especieCompartilhada));
            }

            // CÁLCULO TEÓRICO DE MEMÓRIA 
            // Estimativa de bytes:
            // Cada String/Referência pesada de dados visuais simulada como 1000 bytes (1KB)
            int tamanhoDadosEspecie = 2000; // Textura + Sprite = ~2000 bytes
            int tamanhoDadosIndividuais = sizeof(float) * 4 + sizeof(int); // X, Y, Altura, Diametro (16 bytes) + Galhos (4 bytes) = 20 bytes + 8 bytes da referência do objeto = 28 bytes

            // Cenário COM Flyweight
            long memoriaEspeciesComFlyweight = fabrica.TotalEspeciesCriadas() * tamanhoDadosEspecie;
            long memoriaArvoresComFlyweight = quantidadeArvores * tamanhoDadosIndividuais;
            long memoriaTotalComFlyweight = memoriaEspeciesComFlyweight + memoriaArvoresComFlyweight;

            // Cenário SEM Flyweight (Multiplicar tudo por cada árvore individual)
            long memoriaTotalSemFlyweight = quantidadeArvores * (tamanhoDadosEspecie + tamanhoDadosIndividuais);

            Console.WriteLine("\nRELATÓRIO DE MEMÓRIA");
            Console.WriteLine($"Total de espécies únicas criadas em cache: {fabrica.TotalEspeciesCriadas()}");
            Console.WriteLine($"Total de árvores posicionadas no terreno: {terreno.Count}");
            Console.WriteLine($"\nMemória gasta COM Flyweight: {memoriaTotalComFlyweight / 1024.0:F2} KB");
            Console.WriteLine($"Memória gasta SEM Flyweight: {memoriaTotalSemFlyweight / 1024.0:F2} KB");
            
            double economiaPercentual = (1.0 - ((double)memoriaTotalComFlyweight / memoriaTotalSemFlyweight)) * 100;
            Console.WriteLine($"\nECONOMIA DE MEMÓRIA: {economiaPercentual:F2}%");
        }
    }
}
