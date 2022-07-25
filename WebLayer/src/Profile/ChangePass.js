import React, { useState} from "react";
import {variables} from "../Variables/Variables";
import {Button, Form, Modal} from "react-bootstrap";

const ChangePass = (props) => {

    const [password, setPassword] = useState('')
    const [confirmPass, setConfirmPass] = useState('')

    const closeWindow = () => {
        props.handleCloseChangeButton();
        setFieldToEmpty();
    }
    const setFieldToEmpty = () => {
        setPassword('');
        setConfirmPass('');
    }
    const confirmPassword = () => {
        if (validatePassword()) {
            if (confirmPass === password) {
                return true;
            } else {
                alert("Password didnt match");
                return false;
            }
        } else {
            alert("Password must contain at least:\n 8 characters,\n 1 capital letter,\n 1 lower letter,\n 1 special symbol ");
            return false;
        }
    }

    const validatePassword = () => {
        const lowerCaseLetters = /[a-z]/g;
        const upperCaseLetters = /[A-Z]/g;
        const numbers = /[0-9]/g;
        if (password.length >= 8 &&
            password.match(lowerCaseLetters) &&
            password.match(upperCaseLetters) &&
            password.match(numbers))
            return true;
        return false;
    }


    const addUserClicked = () => {

        if (password !== ''
            && confirmPass !== '') {

            if (confirmPassword()) {
                createAndSendToDbUser(password);
                props.refreshPage();
                setFieldToEmpty();
                props.handleCloseChangeButton();
            }

        } else {
            alert("You forgot to fill some fields.")
        }


    }
    const createAndSendToDbUser = (password) => {
        fetch(variables.API_URL + 'Authentication/changepass?newPass='+ password, {
            method: 'POST',
            headers: {
                'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('user')),
                'Content-Type': 'application/json'
            }

        })
            .then((res) => {
                if(!res.ok){
                    throw new Error();
                }
            }).catch(() => {
            alert("Some fields are incorrect or empty");
        });
    }

    return(
        <>
            <Modal show={props.showChangeBtn} onHide={props.handleCloseChangeButton}>
                <Modal.Header closeButton>
                    <Modal.Title className="text-primary text-center">Change Password</Modal.Title>
                </Modal.Header>
                <Modal.Body>
            <div style={{display: 'flex', justifyContent: 'center'}}>
                <Form>

                    <Form.Group className="mb-3">
                        <Form.Label>New Password</Form.Label>
                        <Form.Control type="password"
                                      placeholder="Password"
                                      value={password}
                                      onChange={(e) =>
                                          setPassword(e.target.value)}
                        />
                    </Form.Group>
                    <Form.Group className="mb-3">
                        <Form.Label>Confirm New Password</Form.Label>
                        <Form.Control type="password"
                                      placeholder="Password"
                                      value={confirmPass}
                                      onChange={(e) =>
                                          setConfirmPass(e.target.value)}
                        />
                    </Form.Group>
                </Form>
            </div>
                </Modal.Body>
                    <Modal.Footer>
                    <Button variant="primary"
                            onClick={addUserClicked}>
                        Submit
                    </Button>
                    <Button variant="secondary" onClick={closeWindow}>Close</Button>
                    </Modal.Footer>
            </Modal>
        </>
    );
}
export default ChangePass;