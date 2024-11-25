
import React, { useState } from 'react';
import {
    Modal,
    ModalBackdrop,
    ModalBody,
    ModalCloseButton,
    ModalContent,
    ModalFooter,
    ModalHeader,
} from '@/components/ui/modal';
import { Button, ButtonText } from '@/components/ui/button';
import { Heading } from '@/components/ui/heading';
import { Center } from '@/components/ui/center';
import { AddIcon, CloseIcon, Icon } from '@/components/ui/icon';
import { Text } from 'react-native';

export default function ModalComponent(props: {
    content: any,
    showModal: boolean,
    setShowModal: any
    title?: any,
    footer?: any,
    openbutton?: any,
    buttonstyle?: any,
    className?: string
}) {
    return (
        <Center className={props.className}>
            {props.openbutton ?
                <Button className={props.buttonstyle ? props.buttonstyle : "px-5 py-7 bg-primary-500"} onPress={() => props.setShowModal(true)}>
                    {props.openbutton}
                </Button>
            : null}

            <Modal
                isOpen={props.showModal}
                onClose={() => {
                    props.setShowModal(false)
                }}
                size="lg"
            >
                <ModalBackdrop />
                <ModalContent>
                    <ModalHeader>
                        <Heading size="lg" className="text-typography-950">
                            {props.title}
                        </Heading>
                        <ModalCloseButton
                            onPress={() => {
                                props.setShowModal(false)
                            }}
                        >
                            <Icon
                                as={CloseIcon}
                                size="md"
                                className="stroke-background-400 group-[:hover]/modal-close-button:stroke-background-700 group-[:active]/modal-close-button:stroke-background-900 group-[:focus-visible]/modal-close-button:stroke-background-900"
                            />
                        </ModalCloseButton>
                    </ModalHeader>
                    <ModalBody>
                        {props.content}
                    </ModalBody>
                    <ModalFooter>
                        {props.footer}
                    </ModalFooter>
                </ModalContent>
            </Modal>
        </Center>
    )
}