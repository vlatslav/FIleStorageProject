import React, {useState} from "react";
import {Button, Modal} from "react-bootstrap";


function EditUser(props) {

    const [email, setEmail] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [nickName, setNickName] = useState('');


    const closeWindow = () => {
        props.handleCloseEditButton()
        setFieldToEmpty()
    }
    const setFieldToEmpty = () => {
        setFirstName('')
        setNickName('')
        setLastName('')
        setEmail('')

    }

    const updateUserClicked = () => {

        if (email !== ''
            && firstName !== ''
            && lastName !== ''
            && nickName !== '') {
            if(validEmail() && validUserName()) {

                props.updateUser(email, firstName, lastName, nickName)
                props.refreshPage()
                setFieldToEmpty()
                props.handleCloseEditButton()

            }
        } else {
            alert("You forgot to fill some fields.")
        }
    }


    const validEmail = () =>
    {
        if(props.users.find(user => user.email === email) === undefined)
        {
            return true
        }
        else {
            alert("Email is already taken")
        }
    }

    const validUserName = () =>
    {
        if(props.users.find(user => user.nickName === nickName) === undefined)
        {
            return true
        }
        else {
            alert("UserName is already taken")
        }
    }

    if(props.currentuser !== undefined) {
        return (
            <>
                <Modal show={props.showEditBtn} onHide={props.handleCloseEditButton}>
                    <Modal.Header closeButton>
                        <Modal.Title>Update User</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <div>
                            <h5>Enter Email</h5>
                            <input type="text" className="form-control"
                                   value={email}
                                   onChange={(e) =>
                                       setEmail(e.target.value)}
                            />
                            <h5>Enter First Name</h5>
                            <input type="text" className="form-control"
                                   value={firstName}
                                   onChange={(e) =>
                                       setFirstName(e.target.value)}
                            />
                            <h5>Enter Last Name</h5>
                            <input type="text" className="form-control"
                                   value={lastName}
                                   onChange={(e) =>
                                       setLastName(e.target.value)}
                            />
                            <h5>Enter Username</h5>
                            <input type="text" className="form-control"
                                   value={nickName}
                                   onChange={(e) =>
                                       setNickName(e.target.value)}
                            />
                        </div>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={
                            closeWindow
                        }>
                            Close
                        </Button>
                        <Button variant="primary" onClick={updateUserClicked}>
                            Update
                        </Button>
                    </Modal.Footer>
                </Modal>


            </>
        )
    }
    else return (<></>)

}

export default EditUser;