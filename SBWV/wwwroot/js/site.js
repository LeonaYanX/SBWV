window.onload = function () {

    let links = document.getElementsByClassName('myLink');

    for (let i = 0; i < links.length; i++) {


        links[i].addEventListener('click', function (event) {

            if (confirm('Вы действительно хотите удалить эту книгу?')) {

            }
            else {
                event.preventDefault();
            }
        });
    }

};