<img src="media-manager-music.ico" alt="drawing" width="256" />

# Maxsys Media Manager 

**Maxsys Media Manager** é uma aplicação desenvolvida em C# para gerenciamento de bibliotecas de música, documento e imagem.
Atualmente, apenas o gerenciamento de músicas foi desenvolvido.

Essa aplicação tem como principal objetivo (além de ser útil para gerenciar meus próprios arquivos), servir de plataforma para o desenvolvimento de habilidades em programação.
Através dela, ponho em prática as técnicas e conhecimentos que venho adquirindo ao longo do processo de aprendizado em desenvolvimento de sistemas.

Como é de se esperar, não é uma aplicação completa, possui muitos padrões diferentes, e pode (e deve) conter erros/gambiarras/bugs.

Por fim, **Maxsys Media Manager** foi criado ***por mim e para mim como cliente final***. Portanto atende (ou quase) meus requisitos para o sistema em questão.


## Maxsys Media Manager - Music

É o único projeto funcional até o momento.
Um código antigo (😳 vergonha!!!) que vem sendo melhorado aos poucos (bem aos poucos).
Logo, há partes bem codificadas, e outras... melhor nem falar.

Desenvolvido em WPF, tem como finalidade, o gerenciamento de minha biblioteca de músicas. Através dele, é possível cadastrar e catalogar músicas *.mp3* e organizá-las de acordo com as informações, bem como gravar no arquivo suas *tags id3v2* (para utilização em outros softwares de música como o Windows Media Player).
Também será possível exportar playlists (.wpl e .m3u). No momento não foram desenvolvidas as telas de gerenciamento de playlists.

- A aplicação cadastra um ***Catálogo***.
	Por exemplo, "ROCK" para músicas de todos os estilos de rock, ou "NACIONAL" para músicas nacionais...
- Cada catálogo possui uma lista de ***Artistas***. 
	Por exemplo, em meu banco de dados encontrarei "Iron Maiden" e "Angra" no catálogo "ROCK", e "Engenheiros do Hawaii" e "Raimundos" no catálogo "NACIONAL".
- Cada artista possui uma lista de ***Álbuns*** (cds).
	Possui informações como seu tipo, que pode ser por exemplo, "Estúdio", "Ao vivo" ou "Compilação"; e ano do álbum.
- Como se espera, os álbuns por fim, tem uma lista de ***Músicas***.
	As músicas são os aquivos *.mp3* e possuem informações relevantes (para mim, cliente final), como, "é cover?", "vocal masculino ou feminino?", etc...


### Como funciona?

1. Arrastam-se os arquivos .mp3 ~~baixados~~ para a tela de Registrar Músicas.

2. Após isso, se edita individualmente o nome da faixa, o número da faixa e se informam os dados obrigatórios (alguns dados são possíveis de se editar em lote).

3. Cada arquivo precisa estar vinculado a um álbum, que por sua vez, em sua própria tela de cadastro foi vinculado à um artista, que por sua vez, em sua própria tela de foi vinculado a um catálogo.

4. Os arquivos devidamente cadastrados, são movidos para a biblioteca de música em sua devida pasta, bem como são gravadas as tags id3v2 em cada arquivo de acordo com as informações cadastradas.

Validações no modelo são realizadas e informadas ao usuário em caso de falha.

------------

O exemplo de um arquivo editado e movido para a biblioteca:

* Arquivo: `<Downloads/iron-maiden - wasted_years (256kpbs).mp3>` arrastado para tela de cadastro.

* Dados editados:
	* Título: `<Wasted Years>`
	* Faixa: `<2>`
	* Álbum`<Somewhere In Time>` selecionado.

		* Infos do Álbum (já cadastrado no álbum):
			* Nome: `<Somewhere In Time>`
			* Ano:`<1986>`
			* Tipo:`<Studio>`
			* Gênero (Musical): `<Heavy Metal>`
			* Artista: `<Iron Maiden>`

				* Infos do Artista (já cadastrado no artista):
					* Nome: `<Iron Maiden>`
					* Catálogo:`<ROCK>`

* Ao registrar:
	* As tags serão inseridas no arquivo
	* o aquivo será renomeado e movido para o caminho:
	`<Musics/ROCK/Iron Maiden/Studio/(1986) Somewhere In Time/02 Wasted Years.mp3>`

Pronto, uma nova música foi colocada na biblioteca.

## ✒️ Autor

* [@MaxDolabella](https://www.github.com/MaxDolabella)

## 🧐 Aprendizagem

Através desse projeto, tenho a oportunidade de por em prática parte do conhecimento adquirido. Obviamente, ainda é limitado, mas a intenção é sempre buscar a melhora.

## 🗝 Licença

[![License](https://img.shields.io/apm/l/atomic-design-ui.svg?)](LICENSE)

## 📧 Feedback

Quaisquer sugestões ou outro contato, escreva-me em maxsystech@outlook.com.