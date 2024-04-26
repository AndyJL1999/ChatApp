const form = document.getElementById('groupForm');
const numberInput = document.getElementById('phoneNumber');
const addNumberButton = document.getElementById('addNumberButton');
const createButton = document.getElementById('createButton');
const listContainer = document.getElementById('numberList');
const phoneNumbers = JSON.parse((document.getElementById('numbers').value));

addNumberButton.addEventListener("click", function (event) {
    phoneNumbers.push(numberInput.value);

    let listItem = document.createElement('li');
    listItem.className = 'list-group-item border-0 border-bottom';

    let spanItem = document.createElement('span');
    spanItem.className = 'text-black-50';
    spanItem.innerHTML = '(' + numberInput.value.substr(0, 3) + ') ' + numberInput.value.substr(3, 3) + '-' + numberInput.value.substr(6, 4);

    listItem.appendChild(spanItem);
    listContainer.appendChild(listItem);

    event.preventDefault();
});

createButton.addEventListener("click", function (event) {

    document.getElementById('numbers').value = phoneNumbers;
    form.submit();

    event.preventDefault();
});