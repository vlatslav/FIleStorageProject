
import React, {useState} from "react";
import {variables} from "../Variables/Variables";
import AddFile from "../AddFile/AddFile";
import UpdateFile from "../EditFileComponent/UpdateFile";


function Posts({ posts, loading, refreshPage, category,users }) {
    const [currentFile,setCurrentFile] = useState();
    const [showEdit, setShowEdit] = useState(false);
    const [show,setShow] = useState(false);
    const handleCloseEdit = () => setShowEdit(false);
    const handleShow = () => setShow(true);
    const handleShowEdit = () => setShowEdit(true);
    const handleClose = () => setShow(false);
    const linkRef = React.createRef();
    const editFile = (fl) => {
        setCurrentFile(fl);
        if(fl !== undefined)
        {
            handleShowEdit();
        }
    }
    const deleteFile = (fileId) => {
        const token = "Bearer " + JSON.parse(localStorage.getItem('user'));
        if (window.confirm('Are you sure?')) {
            fetch(variables.API_URL + 'File/' + fileId, {
                method: 'DELETE',
                headers: {
                    'Authorization': token,
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })
                .then((res) => {
                    if(!res.ok){
                        throw new Error();
                    }
                    refreshPage();
                }).catch((error) => {
                    alert('Failed');
                })
        }

    }
    const downloadfile2 = (fileId) => {
        const file = posts?.filter(x => x.fileId === fileId);
        fetch(
            variables.API_URL + "File/downloadfile/" + fileId,
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            }
        ).then(res => {
            if(!res.ok){
                throw new Error();
            }
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
    if (loading) {
        return <h2>Loading...</h2>;
    }
    return (
        <>
            {localStorage.getItem('UserId') !== null &&
            <button variant="primary" className="btn btn-primary m-2"
                    onClick={handleShow}>
                AddFile
            </button>}

            <table className="content-table">
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
                    <th>
                        Nickname
                    </th>
                </tr>

                </thead>
                <tbody>
                {posts?.map(file =>
                    <tr key={file.fileId}>
                        <td>{posts?.indexOf(file) + 1}</td>
                        <td>{category?.find(ctg => ctg.categoryId === file.categoryId).categoryName}</td>
                        <td><h6>{file.title}</h6></td>
                        <td>{file.description}</td>
                        <td>{users?.find(u => u.userId === file.userId).userName}</td>
                        <td><a ref={linkRef}/></td>
                        <td>
                            <button className="btn btn-outline-success m-lg-1" type="button" onClick={() => downloadfile2(file.fileId)}>Download</button>

                            {file.userId === localStorage.getItem("UserId") && localStorage.getItem("UserId") !== null
                            || localStorage.getItem("roles")?.includes("Administrator") ?
                            <button className="btn btn-outline-danger m-lg-1" type="button" onClick={() => deleteFile(file.fileId)}>Delete</button>
                                : null}
                            {file.userId === localStorage.getItem("UserId") && localStorage.getItem("UserId") !== null
                            || localStorage.getItem("roles")?.includes("Administrator") ?
                            <button className="btn btn-outline-info m-lg-1" type="button" onClick={editFile.bind(this, file)}>Update</button>
                            : null}
                        </td>
                    </tr>
                )}
                </tbody>
            </table>
            <AddFile
                handleClose={handleClose}
                handleShow={handleShow}
                show={show}
                refreshPage={refreshPage}
                files={posts}
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

export default Posts;