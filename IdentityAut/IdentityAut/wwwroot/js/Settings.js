let userPrfile = document.getElementById('userProfile')
userPrfile.addEventListener('change', () => {

    let picture = event.target.files[0];

    if (!picture.type.match(/^image\/jpeg/)) {
        let badImage = document.getElementById('image-validation');
        badImage.textContent = 'Принимаютя только jpeg изображения';
        return;
    }
    if (picture.size > 2097152) {
        let badImage = document.getElementById('image-validation');
        badImage.textContent = 'Изображение весит более 2 мб';
        return;
    }

    function readFileAsByteArray(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsArrayBuffer(file);
            reader.onload = (event) => {
                const arrayBuffer = event.target.result;
                const byteArray = new Uint8Array(arrayBuffer);
                resolve(byteArray);
            };
            reader.onerror = () => {
                reject(reader.error);
            };
        });
    }

        let fileReader = new FileReader();
        fileReader.readAsDataURL(picture);

        fileReader.onload = async function (event)
        {
            let image = new Image();

            image.src = event.target.result;

            image.onload = async function () {

                if ((this.width > 200 && this.height > 200) & (this.width < 1920 & this.height < 1080)) {

                    let imgElement = document.getElementById("image");
                    imgElement.src = event.target.result;
                    let badImage = document.getElementById('image-validation');
                    badImage.textContent = '';

                    let byteArray = await readFileAsByteArray(picture);
                    const base64String = btoa(String.fromCharCode(...byteArray));
                    await fetch('/Settings/GetUserByteArrayPicture', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify(base64String)
                    });
                }
                else {
                    let badImage = document.getElementById('image-validation');
                    badImage.textContent='Принимаютя изображения от 200*200 до 1600*1200';
                }

            }

        }
});

