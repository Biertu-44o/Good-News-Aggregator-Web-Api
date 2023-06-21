async function getArticleNamesData() {

    let artcleNamesResponse = await fetch(
        'https://localhost:7110/Article/GetArticlesNames');

    let data = await artcleNamesResponse.json();

    const ac = new Autocomplete(document.getElementById('article-search'), {
        data: data,
        maximumItems: 2,
        treshold: 3,
        onSelectItem: ({ label, value }) => {
            console.log("article selected:", label, value);
            location.replace(`/article/GetSelectedArticle?ArticleId=${value}`);


        },
        highlightTyped: true,
        highlightClass: 'text-warning'

    });
}

getArticleNamesData();