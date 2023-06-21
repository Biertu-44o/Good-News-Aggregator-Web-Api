let articleCards = document.querySelectorAll('.article-card');

articleCards.forEach(articleCard => {
    let preview = articleCard.querySelector('.preview');
    let fullArticle = articleCard.querySelector('.full');

    articleCard.addEventListener('mouseover', () => {
        preview.classList.add('magictime', 'puffOut');
        fullArticle.classList.remove('hidden','magictime', 'puffOut');
        fullArticle.classList.add('magictime', 'puffIn');
    });

    articleCard.addEventListener('mouseout', () => {
        preview.classList.remove('magictime', 'puffOut');
        preview.classList.add('magictime', 'puffIn');
        fullArticle.classList.add('magictime', 'puffOut');
    });
});