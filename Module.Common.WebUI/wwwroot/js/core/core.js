function clearInputGroup(groupButton) {
    const input = groupButton.closest('.input-group').querySelector('input');
    input.value = '';
    input.dispatchEvent(new Event("keyup"))
}