# Simulador de Árvores com Flyweight 

Este projeto foi feito para a atividade de Padrões de Projeto, onde precisava simular a distribuição de milhares de árvores em um terreno de um jogo 2D, mas focando em economizar o máximo de memória RAM possível usando o padrão **Flyweight**.

## O Problema
O desafio proposto era colocar **50.000 árvores** de **50 espécies diferentes** no mapa. 
Cada árvore tem coisas que são iguais para todo mundo da mesma espécie (como o nome da espécie, a textura do tronco e o sprite das folhas, que são arquivos pesados) e coisas que mudam de árvore para árvore (como a posição X/Y no mapa, a altura, o diâmetro e o número de galhos).

Se a gente criasse um objeto completo para cada uma das 50.000 árvores, duplicando essas texturas pesadas na memória o tempo todo, o jogo ia travar por falta de RAM.

## Como apliquei o Flyweight 

Para resolver isso, dividi o código exatamente como manda o padrão Flyweight:

1. **Estado Intrínseco (O que não muda):** Criei a classe `EspecieArvore`. Ela guarda as texturas e os dados pesados das espécies. Graças à fábrica (`EspecieFactory`), cada uma das 50 espécies é criada **só uma vez** na memória e fica guardada em um cache.
2. **Estado Extrínseco (O que muda):** Criei a classe `ArvoreIndividual`. Cada árvore do mapa só guarda as suas informações leves (coordenadas, altura, diâmetro e galhos) e um ponteiro apontando para a espécie dela que já está no cache.

## Economia de Memória
Quando você roda o programa no terminal, ele faz o cálculo matemático exato com base nos bytes das variáveis:

* **Sem Flyweight:** Se a gente clonasse os dados pesados das espécies para as 50.000 árvores, o consumo de RAM ia lá para cima.
* **Com Flyweight:** Como a gente reaproveita as 50 espécies e só cria as variáveis leves das posições, a memória fica super otimizada.

**O resultado final no console mostra uma economia de mais de 90% de memória RAM!**

## Como testar e rodar
Para rodar a simulação e ver o relatório de memória no terminal, é só abrir a pasta no VS Code e digitar:
```bash
dotnet run
