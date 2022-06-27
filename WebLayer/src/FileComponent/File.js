import React, {useEffect, useState} from "react";
import {variables} from "../Variables/Variables";
import AddFile from "../AddFile/AddFile";
import UpdateFile from "../EditFileComponent/UpdateFile";

function File() {
    const [files,setFiles] = useState();
    const [currentFile,setCurrentFile] = useState();
    const [showEdit, setShowEdit] = useState(false);
    const [show,setShow] = useState(false);
    const [category,setCategory] = useState();
    const [refresh, setRefresh] = useState(false);
    const handleCloseEdit = () => setShowEdit(false);
    const handleShow = () => setShow(true);
    const handleShowEdit = () => setShowEdit(true);
    const handleClose = () => setShow(false);
    const linkRef = React.createRef();
    useEffect(() => {
        fetch(variables.API_URL + 'File/files')
            .then(response => response.json())
            .then(data => {
                setFiles(data);
                setRefresh(false);
            })
    }, [refresh])
    useEffect(() => {
        fetch(variables.API_URL + 'Category')
            .then(response => response.json())
            .then(data => {
                setCategory(data);
                setRefresh(false);
            })
    }, [refresh])
    const refreshPage = () => {
        setRefresh(true)
    }
    const editBook = (fl) => {
        setCurrentFile(fl);
        if(fl !== undefined)
        {
            handleShowEdit();
        }
    }
    const deleteBook = (fileId) => {
        const token = "Bearer " + JSON.parse(localStorage.getItem('user'));
        console.log(token);
        if (window.confirm('Are you sure?')) {
            fetch(variables.API_URL + 'File/' + fileId, {
                method: 'DELETE',
                headers: {
                    'Authorization': token,
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })

                .then(res => res.json())
                .then((result) => {
                    refreshPage()
                }, (error) => {
                    alert('Failed');
                })
        }
    }
    const downloadfile2 = (fileId) => {
        const file = files?.filter(x => x.fileId === fileId);
        fetch(
            variables.API_URL + "File/downloadfile/" + fileId,
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            }
        ).then(res => {
            return res.blob();
        }).then(blob => {
            const href = window.URL.createObjectURL(blob);
            const a = linkRef.current;
            a.download = file.fileName;
            a.href = href;
            a.click();
            a.href = '';
        }).catch(err => console.error(err));
    }
    console.log(category);
    if (files?.length !== 0) {
        return (
            <>
                {localStorage.getItem('UserId') !== null &&
                <button variant="primary" className="btn btn-primary m-2 float-end"
                        onClick={handleShow}>
                    AddFile
                </button>}

                <table>
                    <thead>
                    <tr>
                        <th>
                            Id
                        </th>
                        <th>
                            Category
                        </th>
                        <th>
                            Title
                        </th>
                        <th>
                            Description
                        </th>
                    </tr>

                    </thead>
                    <tbody>
                    {files?.map(file =>
                        <tr key={file.fileId}>
                            <td>{files?.indexOf(file) + 1}</td>
                            <td>{category?.find(ctg => ctg.categoryId === file.categoryId).categoryName}</td>
                            <td>{file.title}</td>
                            <td>{file.description}</td>
                            <td>
                                <button type="button" onClick={() => downloadfile2(file.fileId)}>Download</button>
                            </td>
                            {file.userId === localStorage.getItem('UserId') && localStorage.getItem('UserId') !== null
                                || localStorage.getItem('roles')?.includes("Administrator") &&
                                <td>
                                    <button type="button" onClick={() => deleteBook(file.fileId)}>Delete</button>
                                </td>
                            }
                            {file.userId === localStorage.getItem('UserId') && localStorage.getItem('UserId') !== null
                                || localStorage.getItem('roles')?.includes("Administrator") &&
                                <td>
                                    <button type="button" onClick={editBook.bind(this, file)}>Update</button>
                                </td>
                            }
                            <td><a ref={linkRef}/></td>
                        </tr>
                    )}
                    </tbody>
                </table>
                <AddFile
                    handleClose={handleClose}
                    handleShow={handleShow}
                    show={show}
                    refreshPage={refreshPage}
                    files={files}
                />
                <UpdateFile
                    refreshPage={refreshPage}
                    showEdit={showEdit}
                    handleCloseEdit={handleCloseEdit}
                    handleShowEdit={handleShowEdit}
                    currentFile={currentFile}
                    category={category}
                />
            </>
        );
    }
    else{
        return (<button className="btn btn-primary" disabled>
            <span className="spinner-border spinner-border-sm"></span>
            Loading..
        </button>)
    }
}

export default File;