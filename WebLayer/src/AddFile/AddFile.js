import GetCategoryId from "./GetCategoryId";
import {variables} from "../Variables/Variables";
import {Button, Dropdown, Modal} from "react-bootstrap";
import React, {useState} from "react";
import './AddFile.css';
import AddTitle from "./AddTitleAndDesc";

function AddFile(props) {

    const [selectedFile, setSelectedFile] = useState();
    const [categoryId, setcategoryId] = useState();
    const [handle, setHandle] = useState(false);
    const [showEdit, setShowEdit] = useState(false);
    const handleCloseEdit = () => setShowEdit(false);
    const handleShowEdit = () => setShowEdit(true);

    const handlerOff = () => {
        setHandle(false);
    };


    const changeHandler = (event) => {
        setSelectedFile(event.target.files[0]);
    };
    const closeWindow = () => {

        if(selectedFile === undefined) {
            props.handleClose();
        }
        else {
            props.handleClose();
            setSelectedFile(null);
            setcategoryId("SelectCategory");
        }
    }
    const handleSubmission = () => {
        if(categoryId !== "SelectCategory" && selectedFile !== null) {
            const formData = new FormData();
            formData.append('File', selectedFile);
            const token = 'Bearer ' + JSON.parse(localStorage.getItem('user'));
            fetch(
                variables.API_URL + "File/uploadfile/" + categoryId,
                {
                    method: 'POST',
                    body: formData,
                    headers: {
                        'Authorization': token
                    }
                }
            )
                .then((result) => {
                    if(!result.ok){
                        throw new Error();
                    }
                    console.log('Success:', result);
                    setHandle(true);
                    props.handleClose();
                    handleShowEdit();
                })
                .catch((error) => {
                    console.error('Error:', error);
                });
        }else{
            alert("You forgot to fill some fields.");
        }
    };
    const setValueToCat = (text) => {
        const idout = GetCategoryId(text);
        setcategoryId(idout);
    };

    return (
        <>
            <Modal show={props.show} onHide={closeWindow}>
                <Modal.Header closeButton>
                    <Modal.Title>Add File</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div style={{width: '100%', height: '100%'}}>
                        <h6 style={{paddingBottom: '1%'}}>Select FileCategory</h6>

                        <Dropdown>
                            <Dropdown.Toggle variant="primary" id="dropdown-basic">
                                File Category
                            </Dropdown.Toggle>

                            <Dropdown.Menu>
                                {props.categories?.map(categ =>
                                    <Dropdown.Item>
                                        <div
                                            onClick={(e) =>
                                                setValueToCat(e.target.textContent)}
                                        >{categ.categoryName}
                                        </div>
                                    </Dropdown.Item>
                                )}
                            </Dropdown.Menu>
                        </Dropdown>
                        <h6 style={{paddingTop: '2%'}}>Select file from your computer</h6>
                        <input style={{paddingTop: '1%', paddingBottom: '2%'}} type="file" name="file" onChange={changeHandler} />
                    </div>
                </Modal.Body>
                <div style={{paddingLeft: '3%', paddingBottom: '2%'}}>
                    <Button variant="secondary" onClick={closeWindow}>
                        Close
                    </Button>
                    <Button variant="primary" onClick={handleSubmission}>
                        Submit
                    </Button>
                </div>
            </Modal>
            <AddTitle
                refresh={props.refreshPage}
                show={showEdit}
                handleShow={handleShowEdit}
                handleClose={handleCloseEdit}
                files={props.files}
                handler={handle}
                handlerOff={handlerOff}
            />
        </>

    );
}
export default AddFile;