# FileFinder
ファイル検索ツール

<h3>主な機能</h3>
<ul>
<li>フォルダ内の名前にキーワードを含むファイルを検索する</li>
<li>フォルダ(ルートパス)指定は、「フォルダの参照」ダイアログと、テキストのインクリメントサーチを備える</li>
<li>探索対象は、ファイル名と、フォルダ名及び、その両方。zip書庫内部を含むファイル内部は検索しない</li>
<li>ルートパスと、検索キーワードは、履歴保存・呼び出しが可能。</li>
<li>サブフォルダを検索するかどうかの切り替え</li>
<li>検索結果リストと、当該ファイルを含むフォルダのツリー表示</li>
<li>リストのクリップボードへのコピー</li>
</ul>

<h3>画面機能説明</h3>
<h4>【基本画面】</h4>

![image1](https://user-images.githubusercontent.com/33775885/34511019-32327762-f09c-11e7-9915-af30a6d07b4f.jpg)
<ol>			
<li>	検索するフォルダのルートパスを指定します。	<br>	
	   テキストボックス右端の▽で、前回の履歴を表示・選択出来ます。		</li>
<li>	クリックすると選択中のルートパスの履歴を削除します。		</li>
<li>	クリックするトフォルダの参照ダイアログを表示します。		</li>
<li>	検索対象を「ファイル名」「フォルダ名」「ファイル名・フォルダ名の両方」から選択します。		</li>
<li>	検索キーワードを指定します。	<br>	
	   テキストボックス右端の▽で、前回の履歴を表示・選択出来ます。		</li>
<li>	クリックすると選択中の検索キーワードの履歴を削除します。		</li>
<li>	サブフォルダも検索する場合、☑を入れてください。		</li>
<li>	検索を開始します。		</li>
<li>	検索結果フォルダツリー	<br>	

<ul><li>	　ルート：検索結果ファイルを全件表示します。		</li>
<li>	　各フォルダ：当該フォルダに入っているファイルを表示します。		</li>
<li>	　フォルダ名が青色：検索キーワードに合致するフォルダを表します。		</li>
</ul>

<li>	検索結果リスト【詳細は下記】		<br>
	　フォルダとファイルはアイコンで区別出来ます。<br>		
	　ヘッダーをクリックすると、以下の検索順で並び替えます		<br>
<ul><li>	名前順		</li>
<li>	名前逆順		</li>
<li>	種類順　　（検索対象が、「ファイル名・フォルダ名の両方」の場合のみ）		</li>
<li>	種類逆順　　（検索対象が、「ファイル名・フォルダ名の両方」の場合のみ）		</li>
</ul>
<li>	検索結果全件のフルパスをクリップボードにコピーします。		</li>
<li>	アプリケーションを終了します。		</li>
</ol>

<h4>【検索結果リスト】</h4>
<ul>
  <li>選択項目をダブルクリックすると、その項目を規定のプログラムで開きます。</li>
  <li>右クリックメニュー

![image3](https://user-images.githubusercontent.com/33775885/34511021-329cad30-f09c-11e7-84ce-b7ee83902c65.jpg)
 
 <ul>
     <li>	パスのコピー：該当項目のフルパスをクリップボードにコピーします。	</li>
     <li>	開く：その項目を規定のプログラムで開きます。	</li>
     <li>	場所を開く：該当項目が位置するフォルダを開きます。	</li>
     <li>	プロパティ：該当項目のプロパティを開きます。	</li>
  </ul>  
  </li>
 </ul>

<h4>【エラー表示】</h4>

![image2](https://user-images.githubusercontent.com/33775885/34511020-326c84de-f09c-11e7-8a2d-cda20668fe45.jpg)

<ul>
  <li>検索処理中にエラーが発生すると「エラー」ボタンが出現します。</li>
  <li>クリックすると、「エラーメッセージ」ウィンドウが表示されます。</li>
</ul>

<h4>【履歴コンボボックス】</h4>

![image4](https://user-images.githubusercontent.com/33775885/34511022-32cfac30-f09c-11e7-9b90-b591b1c8d5d9.jpg)

 <ul>
   <Li>ルートパスとキーワード欄のテキストボックスは右端の▽を押すと、前回入力された項目が表示されます。</li>
   <li>選択後、更に右隣の「×」ボタンをクリックすることで、該当の履歴を削除出来ます。</li>
  </ul>
  
<h4>【フォルダパスのインクリメントサーチ】</h4>

![image5](https://user-images.githubusercontent.com/33775885/34511023-3361e49c-f09c-11e7-97d1-aff8533d612e.jpg)

<ul>
  <li>ルートパス欄に於いて、フォルダパスを入力すると、順次候補が下に出現します。</li>
  <li>Enterキーを押すか、ウィンドウ内のなにもないところをクリックすると、候補欄が消えます。</li>
</ul>

END