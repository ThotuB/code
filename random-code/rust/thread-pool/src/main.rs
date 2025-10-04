use std::{
    sync::{
        mpsc::{self, Sender},
        Arc, Mutex,
    },
    thread::{self, sleep, JoinHandle},
    time::Duration,
};

type Job = Box<dyn FnOnce() + Send + 'static>;

struct ThreadPoolInner {
    workers: Vec<JoinHandle<()>>,
    sender: Sender<Job>,
}

impl ThreadPoolInner {
    fn send(&self, job: Job) {
        self.sender.send(job).unwrap();
    }

    fn join(self) {
        drop(self.sender);

        for worker in self.workers {
            worker.join().unwrap();
        }
    }
}

impl Default for ThreadPoolInner {
    fn default() -> Self {
        Self {
            workers: Vec::new(),
            sender: mpsc::channel::<Job>().0,
        }
    }
}

struct ThreadPool {
    inner: ThreadPoolInner,
}

impl ThreadPool {
    fn new(size: usize) -> Self {
        assert!(size != 0, "thread pool size should not be 0");

        let (tx, rx) = mpsc::channel::<Job>();

        let rx = Arc::new(Mutex::new(rx));

        let mut handles = Vec::with_capacity(size);

        for i in 0..size {
            let rx = Arc::clone(&rx);
            let handle = thread::spawn(move || loop {
                let maybe_job = rx.lock().unwrap().recv();

                match maybe_job {
                    Ok(job) => {
                        println!("Worker {} got a job; executing.", i);

                        job();
                    }
                    Err(_) => {
                        println!("Worker {} is shutting down.", i);
                        break;
                    }
                }
            });
            handles.push(handle);
        }

        let inner = ThreadPoolInner {
            workers: handles,
            sender: tx,
        };

        Self { inner }
    }

    fn execute<T>(&self, job: T)
    where
        T: FnOnce() + Send + 'static,
    {
        self.inner.send(Box::new(job));
    }
}

impl Drop for ThreadPool {
    fn drop(&mut self) {
        println!("Sending terminate signal to all workers.");
        let inner = std::mem::take(&mut self.inner);
        inner.join();
    }
}

fn do_something(text: String) {
    println!("{}", text);
    sleep(Duration::from_secs(2));
}

fn main() {
    let thread_pool = ThreadPool::new(4);

    for _ in 0..16 {
        let text = String::from("Hello, World!");
        thread_pool.execute(|| {
            do_something(text);
        });
    }
    println!("Main thread is done.");
}
