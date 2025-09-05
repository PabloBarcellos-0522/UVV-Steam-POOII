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

### Novos Recursos
- **`💡 PLANEJADO`** **Seleção de Carros:**
 - **Descrição:** Adicionar uma tela de menu onde o jogador pode escolher entre carros com
diferentes atributos (velocidade, aceleração, manobra).
 - **Data de Conclusão:**

<br>

- **`💡 PLANEJADO`** **Menu Principal:**
 - **Descrição:** Tela inicial com as opções "Jogar" e "Sair".
 - **Data de Conclusão:** 

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
 - **Data de Conclusão:** 02/04/2025
   
<br>

- **`⚠️ CORRIGIDO`** **Bola Veloz:**
 - **Descrição:** A bola começa devagar e aumenta MUITO a velocidade com a pontuação
 - **Resolução:** Agora ela aumenta de velocidade com o tempo e reseta para 5px/s quando alguém pontua
 - **Data de Conclusão:** 02/04/2025

<br>

- **`⚠️ CORRIGIDO`** **Simetria Previsível:**
 - **Descrição:** A bola tem um movimentó muito previsível, passando sempre pelos mesmos locais
 - **Resolução:** Adicionamos uma velocidade aleatória no eixo Y quando a bola toca em um jogador, tornando o jogo mais divertido :)
 - **Data de Conclusão:** 02/04/2025

### Novos Recursos
- **`✅ ADICIONADO`** **IA Avançada:**
 - **Descrição:** Aumento de inteligencia da CPU quando a pontuação do player é superior a 5, agora ele segue a bola mais rigorosamente
 - **Data de Conclusão:** 02/04/2025

<br>

- **`✅ ADICIONADO`** **Menu:**
 - **Descrição:** Agora o jogo tem um menu ao ser iniciado, permitindo o início de novas partidas sem precisar abrir novamente o game
 - **Data de Conclusão:** 02/04/2025

<br>

- **`✅ ADICIONADO`** **Modo 2 players:**
 - **Descrição:** Modo de 2 jogadores adicionado ao menu do jogo, te permite jogar com seu amigo e perder vergonhosamente para ele
 - **Data de Conclusão:** 02/04/2025

<br>

- **`✅ ADICIONADO`** **Black and White:**
 - **Descrição:** Feature visual que tansforma os jogadores em preto e branco e mudando a cor da bola para a cor que a acertou
 - **Data de Conclusão:** 02/04/2025

