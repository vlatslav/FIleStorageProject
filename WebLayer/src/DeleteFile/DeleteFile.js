function DeleteFile(props) {
    const deleteBook = (file) => {
        if (window.confirm('Are you sure?')) {
            fetch(variables.API_URL + 'Files/' + file.fileId, {
                method: 'DELETE',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })

                .then(res => res.json())
                .then((result) => {
                    alert(result);
                    props.refreshPage()
                }, (error) => {
                    alert('Failed');
                })
        }else{
            alert("You forgot to fill some fields.");
        }
    }
}
export default DeleteFile;