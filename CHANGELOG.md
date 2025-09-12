# Log de Melhorias e Funcionalidades

Este documento registra todas as novas funcionalidades, aprimoramentos e correções de bugs
implementadas nos jogos do projeto.

**Status:**
- `✅ ADICIONADO` - Funcionalidade implementada e testada.
- `⚠️ CORRIGIDO` - Bug corrigido com sucesso.
- `💡 PLANEJADO` - Ideia para ser implementada no futuro.

---
<br>

## 🏎️ Jogo de Corrida

<h4 align="center">Antes e depois</h4>

<div align="center" style="display: flex; justify-content: space-around">
 <img style="width: 500px" src="./Main Resources/Car_Before.png" />
 <img style="width: 500px" src="./Main Resources/Car_After.png" />
</div>

### Novos Recursos
### Novos Recursos
- **`✅ ADICIONADO`** **Mecânica "Near Miss(quase acidente)":**
 - **Descrição:** O jogador recebe mais pontos ao passar perto de um carro adversário.
 - **Data de Conclusão: 07/09/25 **

<br>

- **` ✅ ADICIONADO`** **Menu Principal:**
 - **Descrição:** Tela inicial com imagem e botão de inicio personalizado.
 - **Data de Conclusão: 07/09/25** 

<br>

- **` ✅ ADICIONADO`** **Incremento no sistema de Pontuação:**
 - **Descrição:** Pontos agora são calculados com base no tempo sobrevivido, carros destruidos no power up "power star" e pela mecânica near miss.
 - **Data de Conclusão: 07/09/25** 

<br>

- **` ✅ ADICIONADO`** **Novo power up: Slow Time :**
 - **Descrição:** Novo power up com modificação de mapa própria. Permite que o player, ao coletar um "floco de neve",pressione "espaço" para diminuir a velocidade dos carros. Pode ser ativado várias vezes até o fim do poder.
 - **Data de Conclusão: 07/09/25** 

<br>

- **` ✅ ADICIONADO`** **Novo power up: Multiplicador de Pontos :**
 - **Descrição:** Novo power up com modificação de mapa própria. Multiplica, em 2 vezes, todos os pontos coletados pelo player certo tempo depois de coletar o poder com icone "2x".
 - **Data de Conclusão: 07/09/25** 

<br>

- **`✅ ADICIONADO`** **Melhorias de Interface :**
 - **Descrição:** Todos os power ups mudam o cenário e o carro com cores diferentes permitindo melhor identificação do fim desses. Melhoria na identificação de carros que o player destruiu.
 - **Data de Conclusão: 07/09/25** 

<br>

- **`⚠️ CORRIGIDO`** **Carro saí pra fora da tela do lado esquerdo da mapa :**
 - **Descrição:** Carro saia para fora do mapa pelo lado direito da tela.
 - **Data de Conclusão: 10/09/25** 


<br>

---
<br>

## 🏓 Jogo de Ping Pong

<h4 align="center">Antes e depois</h4>

<div align="center" style="display: flex; justify-content: space-around">
 <img style="width: 500px" src="./Main Resources/Pong_Before.png" />
 <img style="width: 500px" src="./Main Resources/Pong_After.png" />
</div>

### Bugs
- **`⚠️ CORRIGIDO`** **Bola Quadrada:**
 - **Descrição:** A "bola" do ping pong estava quadrada, o que causava estranheza para os players
 - **Resolução:** Deixamos a bola redonda cortando suas pontas trasformando ela numa esfera
 - **Data de Conclusão:** 02/09/2025
   
<br>

- **`⚠️ CORRIGIDO`** **Bola Veloz:**
 - **Descrição:** A bola começa devagar e aumenta MUITO a velocidade com a pontuação
 - **Resolução:** Agora ela aumenta de velocidade com o tempo e reseta para 5px/s quando alguém pontua
 - **Data de Conclusão:** 02/09/2025

<br>

- **`⚠️ CORRIGIDO`** **Simetria Previsível:**
 - **Descrição:** A bola tem um movimentó muito previsível, passando sempre pelos mesmos locais
 - **Resolução:** Adicionamos uma velocidade aleatória no eixo Y quando a bola toca em um jogador, tornando o jogo mais divertido :)
 - **Data de Conclusão:** 02/09/2025

### Novos Recursos
- **`✅ ADICIONADO`** **IA Avançada:**
 - **Descrição:** Aumento de inteligencia da CPU quando a pontuação do player é superior a 5, agora ele segue a bola mais rigorosamente
 - **Data de Conclusão:** 02/09/2025

<br>

- **`✅ ADICIONADO`** **Menu:**
 - **Descrição:** Agora o jogo tem um menu ao ser iniciado, permitindo o início de novas partidas sem precisar abrir novamente o game
 - **Data de Conclusão:** 02/09/2025

<br>

- **`✅ ADICIONADO`** **Modo 2 players:**
 - **Descrição:** Modo de 2 jogadores adicionado ao menu do jogo, te permite jogar com seu amigo e perder vergonhosamente para ele
 - **Data de Conclusão:** 02/09/2025

<br>

- **`✅ ADICIONADO`** **Black and White:**
 - **Descrição:** Feature visual que tansforma os jogadores em preto e branco e mudando a cor da bola para a cor que a acertou
 - **Data de Conclusão:** 02/09/2025

<br>

---
<br>

## 👽 Jogo Tiros espaciais

<h4 align="center">Antes e depois</h4>

<div align="center" style="display: flex; justify-content: space-around">
 <img style="width: 500px" src="./Main Resources/Space_Before.png" />
 <img style="width: 500px" src="./Main Resources/Space_After.png" />
</div>

### Novos Recursos
- **`✅ ADICIONADO`** **Menu Animado:**
 - **Descrição:** Menu feito com um gif animado com botões de iniciar e sair do jogo
 - **Data de Conclusão:** 08/09/2025

<br>

- **`✅ ADICIONADO`** **Escolha de naves pelo menu:**
 - **Descrição:** Agora o player pode selecionar sua nave inicial diretamente pelo menu do jogo
 - **Data de Conclusão:** 08/09/2025 

<br>

- **`✅ ADICIONADO`** **Aliados ao campo:**
 - **Descrição:** Naves aliadas aparecem junto ao inimigos, não atirar nelas de dá uma pontuação extra
 - **Data de Conclusão:** 10/09/2025 

<br>

- **`✅ ADICIONADO`** **Ceu estrelado:**
 - **Descrição:** Fundo estrelado com movimento para melhorar a impressão de movimento da nave
 - **Data de Conclusão:** 10/09/2025 

<br>

- **`✅ ADICIONADO`** **Boss Abrantes MECA:**
 - **Descrição:** Boss implementado com o rosto do iconico professor Abrantes, mestre dos Psets e sua arte computacional
 - **Data de Conclusão:** 11/09/2025 

<br>

- **`✅ ADICIONADO`** **Ataques implementados:**
 - **Descrição:** O Boss final tem 3 ataques principais implementados, eles são bolas de energia, lazers e cabeçada (já que ele é uma cabeça)
 - **Data de Conclusão:** 11/09/2025 

<br>

- **`✅ ADICIONADO`** **Boca Animada:**
 - **Descrição:** Boss final com animação de ataque ( Ele abre a sua grande boca )
 - **Data de Conclusão:** 11/09/2025 

<br>
